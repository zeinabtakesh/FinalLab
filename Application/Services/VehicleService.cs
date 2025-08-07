using Application.Abstractions.DTOs;
using Application.Abstractions.Interfaces;
using Domain.Entities;
using Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;


public class VehicleService : IVehicleService
    {
        private readonly LabDbContext _db;

        public VehicleService(LabDbContext db) => _db = db;

        public async Task<List<VehicleDto>> GetAllAsync() =>
            await _db.Vehicles
                .Select(v => new VehicleDto
                {
                    Id = v.Id,
                    PlateNumber = v.PlateNumber,
                    Model = v.Model,
                    Status = v.Status
                }).ToListAsync();

        public async Task<VehicleDto?> GetByIdAsync(Guid id)
        {
            var v = await _db.Vehicles.FindAsync(id);
            return v is null ? null : new VehicleDto
            {
                Id = v.Id,
                PlateNumber = v.PlateNumber,
                Model = v.Model,
                Status = v.Status
            };
        }

        public async Task<VehicleDto> CreateAsync(CreateVehicleDto dto)
        {
            var v = new Vehicle
            {
                Id = Guid.NewGuid(),
                PlateNumber = dto.PlateNumber,
                Model = dto.Model,
                Status = dto.Status
            };
            _db.Vehicles.Add(v);
            await _db.SaveChangesAsync();
            return new VehicleDto
            {
                Id = v.Id,
                PlateNumber = v.PlateNumber,
                Model = v.Model,
                Status = v.Status
            };
        }

        public async Task<bool> UpdateAsync(Guid id, CreateVehicleDto dto)
        {
            var v = await _db.Vehicles.FindAsync(id);
            if (v is null) return false;

            v.PlateNumber = dto.PlateNumber;
            v.Model = dto.Model;
            v.Status = dto.Status;

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var v = await _db.Vehicles.FindAsync(id);
            if (v is null) return false;
            _db.Vehicles.Remove(v);
            await _db.SaveChangesAsync();
            return true;
        }
    
}
