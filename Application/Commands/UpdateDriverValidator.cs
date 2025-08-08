namespace Application.Commands;

using Application.Commands;
using FluentValidation;
using Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
public class UpdateDriverValidator : AbstractValidator<UpdateDriverCommand>
    {
        private readonly LabDbContext _db;

        public UpdateDriverValidator(LabDbContext db)
        {
            _db = db;

            RuleFor(d => d.Id)
                .MustAsync((id, ct) => _db.Drivers.AnyAsync(x => x.Id == id, ct))
                .WithMessage("Driver not found.");

            RuleFor(d => d.Name)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(d => d.LicenseNumber)
                .NotEmpty()
                .MaximumLength(20)
                .MustAsync(async (cmd, license, ct) =>
                    !await _db.Drivers.AnyAsync(d => d.LicenseNumber == license && d.Id != cmd.Id, ct))
                .WithMessage("License number must be unique.");
        }
    
}
