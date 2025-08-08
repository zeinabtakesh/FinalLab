using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Infrastructure.External.Auth;

using System.Security.Claims;
using System.Text.Json;
using Application.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

public static class AuthServiceCollectionExtensions
{
    public static IServiceCollection AddAuthServices(this IServiceCollection s, IConfiguration cfg)
    {
        s.AddHttpContextAccessor();

        s.AddHttpClient<IIdentityService, KeycloakIdentityService>(c =>
        {
            var adminBase = cfg["Keycloak:AdminBase"] ?? "http://localhost:8080";
            c.BaseAddress = new Uri(adminBase.TrimEnd('/'));
        });

        s.AddScoped<ICurrentUser, CurrentUser>();
        return s;
    }

    public static IServiceCollection AddJwtAuth(this IServiceCollection s, IConfiguration cfg)
    {
        var section = cfg.GetSection("Keycloak:Jwt");
        s.AddOptions<KeycloakJwtOptions>()
            .Bind(section)
            .ValidateDataAnnotations()
            .Validate(o => !string.IsNullOrWhiteSpace(o.Authority) && !string.IsNullOrWhiteSpace(o.Audience),
                      "Keycloak Jwt options are invalid");

        var opts = section.Get<KeycloakJwtOptions>()!;

        s.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
         .AddJwtBearer(options =>
         {
             options.Authority = opts.Authority.TrimEnd('/');
             options.Audience  = opts.Audience;
             options.RequireHttpsMetadata = bool.TryParse(section["RequireHttps"], out var https) && https;

             options.TokenValidationParameters = new TokenValidationParameters
             {
                 ValidateIssuer = true,
                 ValidIssuer    = opts.Authority,
                 ValidateAudience = true,
                 ValidAudience    = opts.Audience,
                 NameClaimType    = JwtClaimTypes.PreferredUserName, 
                 RoleClaimType    = ClaimTypes.Role
             };

             options.Events = new JwtBearerEvents
             {
                 OnTokenValidated = ctx =>
                 {
                     var id = ctx.Principal?.Identity as ClaimsIdentity;
                     if (id is null) return Task.CompletedTask;

                     var realmAccessJson = ctx.Principal?.FindFirst("realm_access")?.Value;
                     if (!string.IsNullOrEmpty(realmAccessJson))
                     {
                         using var doc = JsonDocument.Parse(realmAccessJson);
                         if (doc.RootElement.TryGetProperty("roles", out var roles) && roles.ValueKind == JsonValueKind.Array)
                         {
                             foreach (var r in roles.EnumerateArray())
                                 id.AddClaim(new Claim(ClaimTypes.Role, r.GetString()!));
                         }
                     }

                     var resourceAccessJson = ctx.Principal?.FindFirst("resource_access")?.Value;
                     if (!string.IsNullOrEmpty(resourceAccessJson))
                     {
                         using var doc = JsonDocument.Parse(resourceAccessJson);
                         if (doc.RootElement.TryGetProperty(opts.Audience, out var client)
                             && client.TryGetProperty("roles", out var cRoles)
                             && cRoles.ValueKind == JsonValueKind.Array)
                         {
                             foreach (var r in cRoles.EnumerateArray())
                                 id.AddClaim(new Claim(ClaimTypes.Role, r.GetString()!));
                         }
                     }

                     foreach (var rc in ctx.Principal!.FindAll(JwtClaimTypes.Roles))
                         id.AddClaim(new Claim(ClaimTypes.Role, rc.Value));

                     return Task.CompletedTask;
                 }
             };
         });

        s.AddAuthorization(options =>
        {
            options.AddPolicy("FleetAdmin", p => p.RequireRole("fleet-admin"));
            options.AddPolicy("FleetUser",  p => p.RequireRole("fleet-user"));
        });

        return s;
    }
}
