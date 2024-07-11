using LoyalitySystem.Application;
using LoyalitySystem.Contracts;
using LoyalitySystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace LoyalitySystem.Host
{
    public static class Dependencies
    {
        public static WebApplicationBuilder AddCustomServices(this WebApplicationBuilder builder)
        {
            var LoyalitySystemDbConnection = builder.Configuration.GetConnectionString("LoyalitySystemDb");

            //register Services
            builder.Services.AddScoped<ILoyalitySystemService, LoyalitySystemService>();

            //register LoyalitySystem db
            builder.Services.AddDbContext<LoyalitySystemDbContext>(options => options.UseSqlServer(LoyalitySystemDbConnection));


            // Configure Serilog
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();
            builder.Host.UseSerilog();
            return builder;

        }
    }
}
