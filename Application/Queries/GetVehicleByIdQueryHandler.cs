using Application.Abstractions.DTOs;

namespace Application.Queries;

using Infrastructure.Persistance;
using MediatR;
using Microsoft.EntityFrameworkCore;


public class GetVehicleByIdQueryHandler : IRequestHandler<GetVehicleByIdQuery, VehicleDto?>
{
    private readonly LabDbContext _db;

    public GetVehicleByIdQueryHandler(LabDbContext db) => _db = db;

    public async Task<VehicleDto?> Handle(GetVehicleByIdQuery request, CancellationToken cancellationToken)
    {
        var v = await _db.Vehicles.FindAsync(new object[] { request.Id }, cancellationToken);
        return v is null ? null : new VehicleDto
        {
            Id = v.Id,
            PlateNumber = v.PlateNumber,
            Model = v.Model,
            Status = v.Status
        };
    }

}
