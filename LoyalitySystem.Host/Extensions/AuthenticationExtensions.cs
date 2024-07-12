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
            var issuerSigningKey = configuration["Keycloak:IssuerSigningKey"];

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
                        Modulus = Base64UrlEncoder.DecodeBytes(issuerSigningKey),
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
