using System.Security.Claims;
using System.Text.Json;
using Application.Behaviors;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Application.Commands;
using Infrastructure.External.Auth;
using Infrastructure.Persistance;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Presentation.Middleware;

IdentityModelEventSource.ShowPII = true;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<LabDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddValidatorsFromAssemblyContaining<CreateDriverValidator>();

builder.Services.Configure<Microsoft.AspNetCore.Mvc.MvcOptions>(options =>
{
    var oldProvider = options.ModelValidatorProviders
        .FirstOrDefault(x => x is FluentValidation.AspNetCore.FluentValidationModelValidatorProvider);
    if (oldProvider != null) options.ModelValidatorProviders.Remove(oldProvider);
});

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(CreateVehicleCommand).Assembly));

builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

builder.Services.AddAuthServices(builder.Configuration);

var keycloak = builder.Configuration.GetSection("Keycloak");

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = keycloak["Authority"];
        options.Audience = keycloak["Audience"];
        options.RequireHttpsMetadata = false;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = keycloak["Authority"],
            ValidateAudience = true,
            ValidAudience = keycloak["Audience"],
            NameClaimType = "preferred_username",
            RoleClaimType = ClaimTypes.Role
        };

        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = ctx =>
            {
                var id = ctx.Principal?.Identity as ClaimsIdentity;
                if (id == null) return Task.CompletedTask;

                var realmAccess = ctx.Principal?.FindFirst("realm_access")?.Value;
                if (!string.IsNullOrEmpty(realmAccess))
                {
                    using var doc = JsonDocument.Parse(realmAccess);
                    if (doc.RootElement.TryGetProperty("roles", out var roles)
                        && roles.ValueKind == JsonValueKind.Array)
                    {
                        foreach (var r in roles.EnumerateArray())
                            id.AddClaim(new Claim(ClaimTypes.Role, r.GetString()!));
                    }
                }

                var resourceAccess = ctx.Principal?.FindFirst("resource_access")?.Value;
                if (!string.IsNullOrEmpty(resourceAccess))
                {
                    using var doc = JsonDocument.Parse(resourceAccess);
                    if (doc.RootElement.TryGetProperty(keycloak["Audience"], out var client)
                        && client.TryGetProperty("roles", out var clientRoles)
                        && clientRoles.ValueKind == JsonValueKind.Array)
                    {
                        foreach (var r in clientRoles.EnumerateArray())
                            id.AddClaim(new Claim(ClaimTypes.Role, r.GetString()!));
                    }
                }

                return Task.CompletedTask;
            },
            OnAuthenticationFailed = ctx =>
            {
                Console.WriteLine("Auth failed: " + ctx.Exception);
                return Task.CompletedTask;
            },
            OnChallenge = ctx =>
            {
                Console.WriteLine("Challenge: " + ctx.ErrorDescription);
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("fleet-admin", p => p.RequireRole("fleet-admin"));
    options.AddPolicy("fleet-user", p => p.RequireRole("fleet-user"));
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "FinalLab API", Version = "v1" });

    var bearerScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter: Bearer {token}",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };

    c.AddSecurityDefinition("Bearer", bearerScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { bearerScheme, Array.Empty<string>() }
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseStatusCodePages();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseMiddleware<ExceptionHandlingMiddleware>();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
