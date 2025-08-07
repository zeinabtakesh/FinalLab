using Application.Abstractions.DTOs;

namespace Application.Commands;

using Domain.Entities;
using Infrastructure.Persistance;
using MediatR;

public class CreateVehicleCommandHandler : IRequestHandler<CreateVehicleCommand, VehicleDto>
{
    private readonly LabDbContext _db;

    public CreateVehicleCommandHandler(LabDbContext db) => _db = db;

    public async Task<VehicleDto> Handle(CreateVehicleCommand request, CancellationToken cancellationToken)
    {
        var vehicle = new Vehicle
        {
            Id = Guid.NewGuid(),
            PlateNumber = request.PlateNumber,
            Model = request.Model,
            Status = request.Status
        };

        _db.Vehicles.Add(vehicle);
        await _db.SaveChangesAsync(cancellationToken);

        return new VehicleDto
        {
            Id = vehicle.Id,
            PlateNumber = vehicle.PlateNumber,
            Model = vehicle.Model,
            Status = vehicle.Status
        };
    }
}
