using System.Text.Json.Serialization;
using MediatR;

namespace Application.Commands;

public class UpdateDriverCommand : IRequest<bool>
{
    [JsonIgnore]
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;
    public string LicenseNumber { get; set; } = string.Empty;
    
}