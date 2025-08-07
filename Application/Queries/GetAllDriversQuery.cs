using Application.Abstractions.DTOs;

namespace Application.Queries;

using MediatR;
using System.Collections.Generic;

public class GetAllDriversQuery : IRequest<List<DriverDto>> { }

