using DoctorUI;
using DoctorUI.Components;
using Helpers.RabbitMQ;
using Polly;
using Polly.Extensions.Http;

var builder = WebApplication.CreateBuilder(args);

var circuitBreakerPolicy = Policy<HttpResponseMessage>
    .Handle<HttpRequestException>()
    .OrResult(response => (int)response.StatusCode >= 500)
    .CircuitBreakerAsync(
        handledEventsAllowedBeforeBreaking: 3,
        durationOfBreak: TimeSpan.FromSeconds(30),
        onBreak: (outcome, timespan) =>
        {
            Console.WriteLine($"Circuit breaker opened due to: {outcome.Exception?.Message ?? outcome.Result?.ReasonPhrase}");
            Console.WriteLine($"Circuit will remain open for {timespan.TotalSeconds} seconds.");
        },
        onReset: () => Console.WriteLine("Circuit breaker reset. Normal operation resumed."),
        onHalfOpen: () => Console.WriteLine("Circuit breaker is half-open. Testing next request.")
    );

// Register HttpClient with Polly policies
builder.Services.AddHttpClient("ExternalServiceClient")
    .AddPolicyHandler(HttpPolicyExtensions.HandleTransientHttpError() // Retry for transient errors
        .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))))
    .AddPolicyHandler(circuitBreakerPolicy); 

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddHttpClient<Service>("ExternalServiceClient");

var rabbitMQSection = builder.Configuration.GetSection("RabbitMQ");
builder.Services.Configure<RabbitMQSettings>(rabbitMQSection);

var app = builder.Build();

var rabbitMQConsumer = app.Services.GetRequiredService<RabbitMQConsumer>();
await Task.Run(() => rabbitMQConsumer.StartListeningAsync());

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
