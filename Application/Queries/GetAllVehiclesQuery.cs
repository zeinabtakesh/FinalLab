using Application.Abstractions.DTOs;

namespace Application.Commands;

using MediatR;

public class GetAllVehiclesQuery : IRequest<List<VehicleDto>> { }

