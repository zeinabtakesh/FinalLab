namespace Application.Commands;

using FluentValidation;
using Application.Commands;
using FluentValidation;
using Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

public class CreateDriverValidator : AbstractValidator<CreateDriverCommand>
    {
        private readonly LabDbContext _db;

        public CreateDriverValidator(LabDbContext db)
        {
            _db = db;

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Driver name is required.")
                .MaximumLength(50);

            RuleFor(x => x.LicenseNumber)
                .NotEmpty().WithMessage("License number is required.")
                .MaximumLength(20)
                .MustAsync(BeUniqueLicenseNumber).WithMessage("License number must be unique.");
        }

        private async Task<bool> BeUniqueLicenseNumber(string licenseNumber, CancellationToken cancellationToken)
        {
            return !await _db.Drivers.AnyAsync(d => d.LicenseNumber == licenseNumber, cancellationToken);
        }
    
}
