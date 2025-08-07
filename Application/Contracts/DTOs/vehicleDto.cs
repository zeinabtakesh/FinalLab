namespace Application.Abstractions.DTOs;

public class VehicleDto
{
    public Guid Id { get; set; }
    public string PlateNumber { get; set; }
    public string Model { get; set; }
    public string Status { get; set; }
}
