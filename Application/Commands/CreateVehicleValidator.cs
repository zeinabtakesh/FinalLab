namespace Application.Commands;

using Application.Commands;
using FluentValidation;

using Application.Commands;
using FluentValidation;
using Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

public class CreateVehicleValidator : AbstractValidator<CreateVehicleCommand>
    {
        private readonly LabDbContext _db;

        public CreateVehicleValidator(LabDbContext db)
        {
            _db = db;

            RuleFor(v => v.PlateNumber)
                .NotEmpty().WithMessage("Plate number is required.")
                .MaximumLength(20)
                .MustAsync(BeUniquePlateNumber).WithMessage("Plate number must be unique.");

            RuleFor(v => v.Model)
                .NotEmpty().WithMessage("Model is required.")
                .MaximumLength(50);

            RuleFor(v => v.Status)
                .NotEmpty().WithMessage("Status is required.")
                .MaximumLength(30);
        }

        private async Task<bool> BeUniquePlateNumber(string plateNumber, CancellationToken cancellationToken)
        {
            return !await _db.Vehicles.AnyAsync(v => v.PlateNumber == plateNumber, cancellationToken);
        }
    
}

