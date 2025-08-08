namespace Application.Commands;
using Application.Commands;
using Infrastructure.Persistance;
using MediatR;
using Microsoft.EntityFrameworkCore;

using Application.Commands;
using Application.Exceptions;
using Infrastructure.Persistance;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class DeleteVehicleCommandHandler : IRequestHandler<DeleteVehicleCommand, bool>
    {
        private readonly LabDbContext _db;
        public DeleteVehicleCommandHandler(LabDbContext db) => _db = db;

        public async Task<bool> Handle(DeleteVehicleCommand request, CancellationToken ct)
        {
            var vehicle = await _db.Vehicles.FirstOrDefaultAsync(x => x.Id == request.Id, ct);
            if (vehicle is null)
                throw new NotFoundException($"Vehicle not found with Id '{request.Id}'.");

            _db.Vehicles.Remove(vehicle);
            await _db.SaveChangesAsync(ct);
            return true;
        }
    
}

