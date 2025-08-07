namespace Domain.Entities;

public class Vehicle
{
    public Guid Id { get; set; }
    public string PlateNumber { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;

    public Driver? Driver { get; set; }
    public Guid? DriverId { get; set; }
}

