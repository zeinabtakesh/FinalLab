namespace Application.Abstractions.DTOs;

public class CreateVehicleDto
{
    public string PlateNumber { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}
