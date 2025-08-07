namespace Application.Commands;

using Infrastructure.Persistance;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class AssignDriverCommandHandler : IRequestHandler<AssignDriverCommand, bool>
    {
        private readonly LabDbContext _db;

        public AssignDriverCommandHandler(LabDbContext db) => _db = db;

        public async Task<bool> Handle(AssignDriverCommand request, CancellationToken cancellationToken)
        {
            var driver = await _db.Drivers.FindAsync(new object[] { request.DriverId }, cancellationToken);
            var vehicle = await _db.Vehicles.FindAsync(new object[] { request.VehicleId }, cancellationToken);

            if (driver == null || vehicle == null) return false;

            var current = await _db.Vehicles.FirstOrDefaultAsync(v => v.DriverId == driver.Id, cancellationToken);
            if (current is not null)
            {
                current.DriverId = null;
            }

            vehicle.DriverId = driver.Id;
            await _db.SaveChangesAsync(cancellationToken);

            return true;
        }
    
}
