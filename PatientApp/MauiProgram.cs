using MeasurementService.Repositories;
using Microsoft.Extensions.Logging;
using PatientService.Repositories;
using Polly.Extensions.Http;
using Polly;

namespace PatientApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

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

            builder.Services.AddHttpClient<Service>("ExternalServiceClient");

            builder.Services.AddMauiBlazorWebView();

            #if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
            #endif

            return builder.Build();
        }
    }
}
