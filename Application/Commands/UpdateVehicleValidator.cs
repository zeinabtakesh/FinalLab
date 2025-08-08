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
            .MustAsync(Exist).WithMessage("Vehicle not found.");

        RuleFor(v => v.PlateNumber)
            .NotEmpty().MaximumLength(20)
            .MustAsync(BeUniquePlate).WithMessage("Plate number must be unique.");

        RuleFor(v => v.Model)
            .NotEmpty().MaximumLength(50);

        RuleFor(v => v.Status)
            .NotEmpty().MaximumLength(30);
    }

    private Task<bool> Exist(Guid id, CancellationToken ct) =>
        _db.Vehicles.AnyAsync(x => x.Id == id, ct);

    private Task<bool> BeUniquePlate(UpdateVehicleCommand cmd, string plate, CancellationToken ct) =>
        _db.Vehicles.AnyAsync(v => v.PlateNumber == plate && v.Id != cmd.Id, ct).ContinueWith(t => !t.Result, ct);
}

