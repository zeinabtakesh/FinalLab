using Application.Abstractions.DTOs;
using Application.Abstractions.Interfaces;
using Domain.Entities;
using Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class DriverService : IDriverService
    {
        private readonly LabDbContext _db;

        public DriverService(LabDbContext db) => _db = db;

        public async Task<List<DriverDto>> GetAllAsync() =>
            await _db.Drivers
                     .Select(d => new DriverDto
                     {
                         Id = d.Id,
                         Name = d.Name,
                         LicenseNumber = d.LicenseNumber
                     }).ToListAsync();

        public async Task<DriverDto?> GetByIdAsync(Guid id)
        {
            var d = await _db.Drivers.FindAsync(id);
            return d is null ? null : new DriverDto
            {
                Id = d.Id,
                Name = d.Name,
                LicenseNumber = d.LicenseNumber
            };
        }

        public async Task<DriverDto> CreateAsync(CreateDriverDto dto)
        {
            var d = new Driver
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                LicenseNumber = dto.LicenseNumber
            };
            _db.Drivers.Add(d);
            await _db.SaveChangesAsync();
            return new DriverDto
            {
                Id = d.Id,
                Name = d.Name,
                LicenseNumber = d.LicenseNumber
            };
        }

        public async Task<bool> UpdateAsync(Guid id, CreateDriverDto dto)
        {
            var d = await _db.Drivers.FindAsync(id);
            if (d is null) return false;

            d.Name = dto.Name;
            d.LicenseNumber = dto.LicenseNumber;

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var d = await _db.Drivers.FindAsync(id);
            if (d is null) return false;

            _db.Drivers.Remove(d);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
