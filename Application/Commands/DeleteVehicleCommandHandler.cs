namespace Application.Commands;
using Application.Commands;
using Infrastructure.Persistance;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class DeleteVehicleCommandHandler : IRequestHandler<DeleteVehicleCommand, bool>
    {
        private readonly LabDbContext _db;
        public DeleteVehicleCommandHandler(LabDbContext db) => _db = db;

        public async Task<bool> Handle(DeleteVehicleCommand request, CancellationToken cancellationToken)
        {
            var vehicle = await _db.Vehicles.FirstOrDefaultAsync(v => v.Id == request.Id, cancellationToken);
            if (vehicle is null) return false;

            _db.Vehicles.Remove(vehicle);
            await _db.SaveChangesAsync(cancellationToken);
            return true;
        }
    
}
