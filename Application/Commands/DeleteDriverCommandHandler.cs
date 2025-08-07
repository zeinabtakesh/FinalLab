namespace Application.Commands;

using Application.Commands;
using Infrastructure.Persistance;
using MediatR;
using Microsoft.EntityFrameworkCore;


public class DeleteDriverCommandHandler : IRequestHandler<DeleteDriverCommand, bool>
    {
        private readonly LabDbContext _db;
        public DeleteDriverCommandHandler(LabDbContext db) => _db = db;

        public async Task<bool> Handle(DeleteDriverCommand request, CancellationToken cancellationToken)
        {
            var driver = await _db.Drivers.FirstOrDefaultAsync(d => d.Id == request.Id, cancellationToken);
            if (driver is null) return false;

            _db.Drivers.Remove(driver);
            await _db.SaveChangesAsync(cancellationToken);
            return true;
        }
    
}
