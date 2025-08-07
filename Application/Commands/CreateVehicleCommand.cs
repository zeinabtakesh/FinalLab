using Application.Abstractions.DTOs;

namespace Application.Commands;

using MediatR;

    public class CreateVehicleCommand : IRequest<VehicleDto>
    {
        public string PlateNumber { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
