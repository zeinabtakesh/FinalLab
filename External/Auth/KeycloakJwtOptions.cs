namespace Infrastructure.External.Auth;

using System.ComponentModel.DataAnnotations;

public sealed class KeycloakJwtOptions
{
    [Required] public string Authority { get; init; } = default!; 
    [Required] public string Audience  { get; init; } = default!; 
}