namespace Application.Commands;

using FluentValidation;
using Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

public class UpdateVehicleValidator : AbstractValidator<UpdateVehicleCommand>
    {
        private readonly LabDbContext _db;

        public UpdateVehicleValidator(LabDbContext db)
        {
            _db = db;

            RuleFor(v => v.Id)
                .NotEmpty().WithMessage("Vehicle ID is required.");

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

        private async Task<bool> BeUniquePlateNumber(UpdateVehicleCommand command, string plateNumber, CancellationToken token)
        {
            return !await _db.Vehicles
                .AnyAsync(v => v.PlateNumber == plateNumber && v.Id != command.Id, token);
        }
    
}
