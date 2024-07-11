using LoyalitySystem.Application;
using LoyalitySystem.Contracts;
using LoyalitySystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

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

            return builder;

        }
    }
}
