using Application.Exceptions;

namespace Application.Commands;

using Infrastructure.Persistance;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class AssignDriverCommandHandler : IRequestHandler<AssignDriverCommand, bool>
    {
        private readonly LabDbContext _db;
        public AssignDriverCommandHandler(LabDbContext db) => _db = db;

        public async Task<bool> Handle(AssignDriverCommand request, CancellationToken ct)
        {
            var driver = await _db.Drivers.FindAsync(new object[] { request.DriverId }, ct);
            if (driver == null)
                throw new NotFoundException($"Driver not found with Id '{request.DriverId}'.");

            var vehicle = await _db.Vehicles.FindAsync(new object[] { request.VehicleId }, ct);
            if (vehicle == null)
                throw new NotFoundException($"Vehicle not found with Id '{request.VehicleId}'.");

            if (vehicle.DriverId == driver.Id)
                throw new ConflictException("This driver is already assigned to this vehicle.");

            var otherVehicle = await _db.Vehicles.FirstOrDefaultAsync(v => v.DriverId == driver.Id, ct);
            if (otherVehicle != null)
                throw new ConflictException($"Driver '{driver.Id}' is already assigned to another vehicle '{otherVehicle.Id}'.");

            vehicle.DriverId = driver.Id;
            await _db.SaveChangesAsync(ct);
            return true;
        
    }
}
