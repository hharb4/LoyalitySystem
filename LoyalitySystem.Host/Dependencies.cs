using FluentValidation.AspNetCore;
using LoyalitySystem.Application;
using LoyalitySystem.Contracts;
using LoyalitySystem.Host.Validators;
using LoyalitySystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace LoyalitySystem.Host
{
    public static class Dependencies
    {
        public static WebApplicationBuilder AddCustomServices(this WebApplicationBuilder builder)
        {
            var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "defaultpassword";
            //var connectionString = string.Format(Configuration.GetConnectionString("LoyalitySystemDb"), dbPassword);

            var LoyalitySystemDbConnection = string.Format(builder.Configuration.GetConnectionString("LoyalitySystemDb"), dbPassword);

            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = builder.Configuration["Redis:ConnectionString"];
            });

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

            // Add FluentValidation
            builder.Services.AddControllers()
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<EarnPointsRequestValidator>());

            return builder;

        }
    }
}
