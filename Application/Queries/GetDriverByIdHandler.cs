namespace Application.Queries;

using Application.Abstractions.DTOs;
using Application.Queries;
using Infrastructure.Persistance;
using MediatR;

public class GetDriverByIdQueryHandler : IRequestHandler<GetDriverByIdQuery, DriverDto?>
    {
        private readonly LabDbContext _db;
        public GetDriverByIdQueryHandler(LabDbContext db) => _db = db;

        public async Task<DriverDto?> Handle(GetDriverByIdQuery request, CancellationToken cancellationToken)
        {
            var d = await _db.Drivers.FindAsync(new object[] { request.Id }, cancellationToken);
            return d is null ? null : new DriverDto
            {
                Id = d.Id,
                Name = d.Name,
                LicenseNumber = d.LicenseNumber
            };
        }
    
}
