using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace TaskService.Api.Extensions;

/// <summary>
///     Extensions for api
/// </summary>
public static class ServiceExtensions
{
    extension(IServiceCollection services)
    {
        internal void ConfigureJwtAuthentication(IConfiguration configuration)
        {
            var authority = configuration["Authentication:Jwt:Authority"];
            if (string.IsNullOrWhiteSpace(authority))
            {
                throw new Exception("Не задан authority");
            }

            var audience = configuration["Authentication:Jwt:Audience"];
            if (string.IsNullOrWhiteSpace(audience))
            {
                throw new Exception("Не задан audience");
            }

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = authority;
                    options.Audience = audience;
                    options.RequireHttpsMetadata = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true
                    };
                });
        }
    }
}
