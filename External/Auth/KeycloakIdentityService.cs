namespace Infrastructure.External.Auth;

using System.Net.Http.Json;
using Application.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

public class KeycloakIdentityService : IIdentityService
{
    private readonly HttpClient _kc;
    private readonly ILogger<KeycloakIdentityService> _log;
    private readonly string _realm;

    public KeycloakIdentityService(HttpClient kc, IConfiguration cfg, ILogger<KeycloakIdentityService> log)
    {
        _kc   = kc;
        _log  = log;
        _realm = cfg["Keycloak:Realm"] ?? "final-lab";
        // NOTE: For real admin calls you usually need a bearer token (client credentials).
        // Add a DelegatingHandler here to attach tokens if needed.
    }

    public async Task<string> CreateUserAsync(string username, string tempPw, string role)
    {
        var user = new
        {
            username,
            email = username,
            enabled = true,
            credentials = new[] { new { type = "password", value = tempPw, temporary = false } }
        };

        var res = await _kc.PostAsJsonAsync($"/admin/realms/{_realm}/users", user);
        res.EnsureSuccessStatusCode();

        var id = res.Headers.Location!.Segments.Last().TrimEnd('/'); // …/users/{id}
        var rolePayload = new[] { new { name = role } };

        var roleRes = await _kc.PostAsJsonAsync(
            $"/admin/realms/{_realm}/users/{id}/role-mappings/realm", rolePayload);
        roleRes.EnsureSuccessStatusCode();

        _log.LogInformation("Created Keycloak user {Id} with role {Role}", id, role);
        return id;
    }

    public async Task DisableUserAsync(string sub)
    {
        var res = await _kc.PutAsJsonAsync($"/admin/realms/{_realm}/users/{sub}", new { enabled = false });
        res.EnsureSuccessStatusCode();
    }
}