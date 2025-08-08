using Application.Exceptions;

namespace Application.Commands;
using Application.Commands;
using Infrastructure.Persistance;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class UpdateVehicleCommandHandler : IRequestHandler<UpdateVehicleCommand, bool>
    {
        private readonly LabDbContext _db;
        public UpdateVehicleCommandHandler(LabDbContext db) => _db = db;

        public async Task<bool> Handle(UpdateVehicleCommand request, CancellationToken ct)
        {
            var vehicle = await _db.Vehicles.FirstOrDefaultAsync(x => x.Id == request.Id, ct);
            if (vehicle is null)
                throw new NotFoundException($"Vehicle not found with Id: {request.Id}");

            var exists = await _db.Vehicles.AnyAsync(v => v.PlateNumber == request.PlateNumber && v.Id != request.Id, ct);
            if (exists)
                throw new ConflictException($"Plate number '{request.PlateNumber}' already exists for another vehicle.");

            vehicle.PlateNumber = request.PlateNumber;
            vehicle.Model = request.Model;
            vehicle.Status = request.Status;

            await _db.SaveChangesAsync(ct);
            return true;
        }
    
}
