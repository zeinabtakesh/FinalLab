namespace Application.Queries;

using Application.Abstractions.DTOs;
using Application.Commands;
using Infrastructure.Persistance;
using MediatR;
using Microsoft.EntityFrameworkCore;


public class GetAllVehiclesQueryHandler : IRequestHandler<GetAllVehiclesQuery, List<VehicleDto>>
{
    private readonly LabDbContext _db;
    public GetAllVehiclesQueryHandler(LabDbContext db) => _db = db;

    public async Task<List<VehicleDto>> Handle(GetAllVehiclesQuery request, CancellationToken cancellationToken)
    {
        return await _db.Vehicles
            .Select(v => new VehicleDto
            {
                Id = v.Id,
                PlateNumber = v.PlateNumber,
                Model = v.Model,
                Status = v.Status
            })
            .ToListAsync(cancellationToken);
    }
    
}
