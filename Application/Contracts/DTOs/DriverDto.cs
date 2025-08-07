namespace Application.Abstractions.DTOs;

public class DriverDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string LicenseNumber { get; set; } = string.Empty;
}
