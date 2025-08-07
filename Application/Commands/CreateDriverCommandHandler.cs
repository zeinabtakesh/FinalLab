using Application.Abstractions.DTOs;

namespace Application.Commands;

using Domain.Entities;
using Infrastructure.Persistance;
using MediatR;


public class CreateDriverCommandHandler : IRequestHandler<CreateDriverCommand, DriverDto>
    {
        private readonly LabDbContext _db;
        public CreateDriverCommandHandler(LabDbContext db) => _db = db;

        public async Task<DriverDto> Handle(CreateDriverCommand request, CancellationToken cancellationToken)
        {
            var driver = new Driver
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                LicenseNumber = request.LicenseNumber
            };

            _db.Drivers.Add(driver);
            await _db.SaveChangesAsync(cancellationToken);

            return new DriverDto
            {
                Id = driver.Id,
                Name = driver.Name,
                LicenseNumber = driver.LicenseNumber
            };
        }
    
}
