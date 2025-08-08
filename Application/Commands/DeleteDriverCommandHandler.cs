using Application.Exceptions;
using Infrastructure.Persistance;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Commands;


public class DeleteDriverCommandHandler : IRequestHandler<DeleteDriverCommand, bool>
    {
        private readonly LabDbContext _db;
        public DeleteDriverCommandHandler(LabDbContext db) => _db = db;

        public async Task<bool> Handle(DeleteDriverCommand request, CancellationToken ct)
        {
            var driver = await _db.Drivers.FirstOrDefaultAsync(x => x.Id == request.Id, ct);
            if (driver is null)
                throw new NotFoundException($"Driver not found with Id '{request.Id}'.");

            _db.Drivers.Remove(driver);
            await _db.SaveChangesAsync(ct);
            return true;
        }
    
}
