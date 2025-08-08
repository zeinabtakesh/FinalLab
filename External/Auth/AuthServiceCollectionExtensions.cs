namespace Infrastructure.External.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Application.Abstractions;
using System;


public static class AuthServiceCollectionExtensions
{
    public static IServiceCollection AddAuthServices(this IServiceCollection s, IConfiguration cfg)
    {
        s.AddHttpContextAccessor();  
        s.AddHttpClient<IIdentityService, KeycloakIdentityService>(c =>
        {
            c.BaseAddress = new Uri(cfg["Keycloak:AdminBase"]);
        });

        s.AddScoped<ICurrentUser, CurrentUser>();
        return s;
    }
}