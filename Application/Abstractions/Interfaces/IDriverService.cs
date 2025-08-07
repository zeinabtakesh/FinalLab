using Application.Abstractions.DTOs;

namespace Application.Abstractions.Interfaces;

public interface IDriverService
{
    Task<List<DriverDto>> GetAllAsync();
    Task<DriverDto?> GetByIdAsync(Guid id);
    Task<DriverDto> CreateAsync(CreateDriverDto dto);
    Task<bool> UpdateAsync(Guid id, CreateDriverDto dto);
    Task<bool> DeleteAsync(Guid id);

}
