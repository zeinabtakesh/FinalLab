namespace Application.Commands;

using MediatR;


public class DeleteVehicleCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public DeleteVehicleCommand(Guid id) => Id = id;
    
}
