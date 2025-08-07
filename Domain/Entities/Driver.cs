using Domain.Entities;
namespace Domain.Entities;

public class Driver
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string LicenseNumber { get; set; } = string.Empty;

    public Vehicle? Vehicle { get; set; }
}

