namespace Application.Commands;

using Application.Commands;
using FluentValidation;
using Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

public class AssignDriverValidator : AbstractValidator<AssignDriverCommand>
    {
        private readonly LabDbContext _db;

        public AssignDriverValidator(LabDbContext db)
        {
            _db = db;

            RuleFor(x => x.DriverId)
                .NotEmpty().MustAsync(DriverExists).WithMessage("Driver does not exist.");

            RuleFor(x => x.VehicleId)
                .NotEmpty().MustAsync(VehicleExists).WithMessage("Vehicle does not exist.");
        }

        private async Task<bool> DriverExists(Guid id, CancellationToken token) =>
            await _db.Drivers.AnyAsync(d => d.Id == id, token);

        private async Task<bool> VehicleExists(Guid id, CancellationToken token) =>
            await _db.Vehicles.AnyAsync(v => v.Id == id, token);
    
}
