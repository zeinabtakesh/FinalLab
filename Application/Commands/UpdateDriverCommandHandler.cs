using Application.Exceptions;

namespace Application.Commands;

using Application.Commands;
using Infrastructure.Persistance;
using MediatR;
using Microsoft.EntityFrameworkCore;
public class UpdateDriverCommandHandler : IRequestHandler<UpdateDriverCommand, bool>
    {
        private readonly LabDbContext _db;
        public UpdateDriverCommandHandler(LabDbContext db) => _db = db;

        public async Task<bool> Handle(UpdateDriverCommand request, CancellationToken ct)
        {
            var driver = await _db.Drivers.FirstOrDefaultAsync(x => x.Id == request.Id, ct);
            if (driver is null)
                throw new NotFoundException($"Driver not found with Id: {request.Id}");

            var exists = await _db.Drivers.AnyAsync(d => d.LicenseNumber == request.LicenseNumber && d.Id != request.Id, ct);
            if (exists)
                throw new ConflictException($"License number '{request.LicenseNumber}' already exists for another driver.");

            driver.Name = request.Name;
            driver.LicenseNumber = request.LicenseNumber;

            await _db.SaveChangesAsync(ct);
            return true;
        }
    
}
