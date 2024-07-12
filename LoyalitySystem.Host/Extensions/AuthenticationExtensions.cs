using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace LoyalitySystem.Host.Extensions
{
    public static class AuthenticationExtensions
    {
        public static IServiceCollection AddCustomAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var keycloakAuthority = configuration["Keycloak:Authority"];
            var keycloakAudience = configuration["Keycloak:Audience"];
            var keycloakClientId = configuration["Keycloak:ClientId"];
            var keycloakClientSecret = configuration["Keycloak:ClientSecret"];

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.Authority = keycloakAuthority;
                options.Audience = keycloakAudience;
                options.RequireHttpsMetadata = false; // dev purposes only
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = keycloakAuthority,
                    ValidateAudience = true,
                    ValidAudiences = new[] { "account" },
                    ValidateLifetime = true,
                    IssuerSigningKey = new RsaSecurityKey(new RSAParameters
                    {
                        Modulus = Base64UrlEncoder.DecodeBytes("mv5K6Hxuq4MHos4lFVG4gQohV4x0OOK4kAgjXcT5385fOfqs9B-2z6CcJThF8MhPF2EFcm3O1M3SMs-Saa27D38qoNZN1rdLSIpI8oF03zZ5yhqn6I6sgOaDHKHrD0v8W58Umfj5zS1WoE8BfF_c1UyO_16Pq9PKfbyCoSBcg_fCCYDZZBsvVfP7dfUqzPtxu0qk8yGjHHy9MecgHe_16sSqCixiZ4sk-ZFsN-JmKu0Q_bKH_bwhSa0B5cg5n-doiOGJ3jaF7pHhJNPWAB6ho1z7RJjS317UENVXhKw_si_3f-VbDCGkibigd0RJChBoffcOIJ0mHvHV8EV_DN_6Tw"),
                        Exponent = Base64UrlEncoder.DecodeBytes("AQAB")
                    })
                };
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        context.Response.Headers.Add("Token-Validation-Errors", context.Exception.Message);
                        return Task.CompletedTask;
                    },
                    OnTokenValidated = context =>
                    {
                        // Log token validated details if needed
                        return Task.CompletedTask;
                    }
                };
            })
            .AddOpenIdConnect("oidc", options =>
            {
                options.Authority = keycloakAuthority;
                options.ClientId = keycloakClientId;
                options.ClientSecret = keycloakClientSecret;
                options.SaveTokens = true;
                options.ResponseType = OpenIdConnectResponseType.Code;
                options.GetClaimsFromUserInfoEndpoint = true;
                options.Scope.Add("openid");
                options.Scope.Add("profile");
                options.RequireHttpsMetadata = false;
            });

            return services;
        }
    }
}
