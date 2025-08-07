using Application.Abstractions.DTOs;

namespace Application.Queries;

using Infrastructure.Persistance;
using MediatR;
using Microsoft.EntityFrameworkCore;


public class GetAllDriversQueryHandler : IRequestHandler<GetAllDriversQuery, List<DriverDto>>
{
    private readonly LabDbContext _db;

    public GetAllDriversQueryHandler(LabDbContext db)
    {
        _db = db;
    }

    public async Task<List<DriverDto>> Handle(GetAllDriversQuery request, CancellationToken cancellationToken)
    {
        return await _db.Drivers
            .Select(d => new DriverDto
            {
                Id = d.Id,
                Name = d.Name,
                LicenseNumber = d.LicenseNumber
            })
            .ToListAsync(cancellationToken);
    }
}

