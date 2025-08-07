namespace Application.Queries;

using Application.Abstractions.DTOs;
using MediatR;

public class GetDriverByIdQuery : IRequest<DriverDto?>
    {
        public Guid Id { get; set; }
        public GetDriverByIdQuery(Guid id) => Id = id;
    
}
