using Application.Abstractions.DTOs;

namespace Application.Queries;
using MediatR;

public class GetVehicleByIdQuery : IRequest<VehicleDto?>
{
    public Guid Id { get; set; }
    public GetVehicleByIdQuery(Guid id) => Id = id;
}
