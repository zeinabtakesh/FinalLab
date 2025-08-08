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
            var v = await _db.Vehicles.FirstOrDefaultAsync(x => x.Id == request.Id, ct);
            if (v is null) return false;

            v.PlateNumber = request.PlateNumber;
            v.Model = request.Model;
            v.Status = request.Status;

            await _db.SaveChangesAsync(ct);
            return true;
        }
    
}
