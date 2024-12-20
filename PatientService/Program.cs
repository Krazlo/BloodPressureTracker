using DB;
using Helpers.RabbitMQ;
using Microsoft.EntityFrameworkCore;
using PatientService.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DB.Context>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 30))
    ));
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
var app = builder.Build();

var rabbitMQSection = builder.Configuration.GetSection("RabbitMQ");
builder.Services.Configure<RabbitMQSettings>(rabbitMQSection);

var rabbitMQConsumer = app.Services.GetRequiredService<RabbitMQConsumer>();
await Task.Run(() => rabbitMQConsumer.StartListeningAsync());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
