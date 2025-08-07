namespace Application.Commands;

using MediatR;

public class UpdateVehicleCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public string PlateNumber { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    
}
