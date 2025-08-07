using Application.Abstractions.DTOs;

namespace Application.Commands;
using MediatR;

using Application.Abstractions.DTOs;
using MediatR;

public class CreateDriverCommand : IRequest<DriverDto>
{
    public string Name { get; set; } = string.Empty;
    public string LicenseNumber { get; set; } = string.Empty;

}

