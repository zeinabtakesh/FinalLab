namespace Application.Commands;
using Application.Commands;
using Infrastructure.Persistance;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class UpdateVehicleCommandHandler : IRequestHandler<UpdateVehicleConmmand, bool>
    {
        private readonly LabDbContext _db;
        public UpdateVehicleCommandHandler(LabDbContext db) => _db = db;

        public async Task<bool> Handle(UpdateVehicleConmmand request, CancellationToken cancellationToken)
        {
            var vehicle = await _db.Vehicles.FirstOrDefaultAsync(v => v.Id == request.Id, cancellationToken);
            if (vehicle is null) return false;

            vehicle.PlateNumber = request.PlateNumber;
            vehicle.Model = request.Model;
            vehicle.Status = request.Status;

            await _db.SaveChangesAsync(cancellationToken);
            return true;
        }
    
}
