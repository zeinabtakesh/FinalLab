namespace Infrastructure.External.Auth;

using System.ComponentModel.DataAnnotations;

public sealed class KeycloakJwtOptions
{
    [Required] public string Authority { get; init; } = default!; // e.g. http://localhost:8080/realms/final-lab
    [Required] public string Audience  { get; init; } = default!; // e.g. final-lab-api
}