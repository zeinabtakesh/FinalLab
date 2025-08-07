using Application.Abstractions.DTOs;

namespace Application.Abstractions.Interfaces;

public interface IVehicleService
{
    Task<List<VehicleDto>> GetAllAsync();
    Task<VehicleDto?> GetByIdAsync(Guid id);
    Task<VehicleDto> CreateAsync(CreateVehicleDto dto);
    Task<bool> UpdateAsync(Guid id, CreateVehicleDto dto);
    Task<bool> DeleteAsync(Guid id);
    
}
