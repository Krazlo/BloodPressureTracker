using DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MeasurementService;

var serviceProvider = new ServiceCollection().AddDbContext<Context>(options =>
options.UseMySql(
                "Server=localhost;Database=mysqlDB;User=sa;Password=S3cr3tP4sSw0rd#123;", new MySqlServerVersion(new Version(8, 0, 23))))
    .AddScoped<IMService, MService>()
    .BuildServiceProvider();

