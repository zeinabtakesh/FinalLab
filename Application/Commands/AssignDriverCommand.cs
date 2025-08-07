namespace Application.Commands;

using MediatR;

public class AssignDriverCommand : IRequest<bool>
    {
        public Guid DriverId { get; set; }
        public Guid VehicleId { get; set; }
    
}
