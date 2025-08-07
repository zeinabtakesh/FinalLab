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

            RuleFor(x => x.Id)
                .NotEmpty();

            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(x => x.LicenseNumber)
                .NotEmpty()
                .MaximumLength(20)
                .MustAsync(BeUnique).WithMessage("License number must be unique.");
        }

        private async Task<bool> BeUnique(UpdateDriverCommand command, string license, CancellationToken token)
        {
            return !await _db.Drivers
                .AnyAsync(d => d.LicenseNumber == license && d.Id != command.Id, token);
        }
    
}
