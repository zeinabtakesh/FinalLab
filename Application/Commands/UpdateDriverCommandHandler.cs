namespace Application.Commands;

using Application.Commands;
using Infrastructure.Persistance;
using MediatR;
using Microsoft.EntityFrameworkCore;
public class UpdateDriverCommandHandler : IRequestHandler<UpdateDriverCommand, bool>
    {
        private readonly LabDbContext _db;
        public UpdateDriverCommandHandler(LabDbContext db) => _db = db;

        public async Task<bool> Handle(UpdateDriverCommand request, CancellationToken cancellationToken)
        {
            var d = await _db.Drivers.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (d is null) return false;

            d.Name = request.Name;
            d.LicenseNumber = request.LicenseNumber;

            await _db.SaveChangesAsync(cancellationToken);
            return true;
        }
    
}
