namespace Infrastructure.External.Auth;

using Application.Abstractions;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

public sealed class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _ctx;
    public CurrentUser(IHttpContextAccessor ctx) => _ctx = ctx;

    private ClaimsPrincipal? P => _ctx.HttpContext?.User;

    public string? Sub =>
        P?.FindFirst("sub")?.Value ?? P?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    
    public string? UserName =>
        P?.FindFirst(JwtClaimTypes.PreferredUserName)?.Value ?? P?.Identity?.Name;

    public string? Email =>
        P?.FindFirst(JwtClaimTypes.Email)?.Value;

    public int? DriverId { get; }

    public bool IsInRole(string role) => P?.IsInRole(role) ?? false;
}