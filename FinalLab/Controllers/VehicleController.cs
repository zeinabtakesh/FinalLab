using Application.Commands;
using Microsoft.AspNetCore.Authorization;

namespace FinalLab.Controllers;

using MediatR;
using Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;


[ApiController]
[Route("api/[controller]")]
public class VehiclesController : ControllerBase
{
    private readonly IMediator _mediator;
    public VehiclesController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    [Authorize(Roles = "fleet-user,fleet-admin")]
    public async Task<IActionResult> GetAll() =>
        Ok(await _mediator.Send(new GetAllVehiclesQuery()));
    
    [HttpGet("{id}")]
    [Authorize(Roles = "fleet-user,fleet-admin")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetDriverByIdQuery(id));
        return result is null ? NotFound() : Ok(result);
    }
    
    [HttpPut("{id}")]
    [Authorize(Roles = "fleet-admin")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateDriverCommand command)
    {
        command.Id = id;

        var success = await _mediator.Send(command);
        return success ? NoContent() : NotFound();
    }



    [HttpDelete("{id}")]
    [Authorize(Roles = "fleet-admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var success = await _mediator.Send(new DeleteVehicleCommand(id));
        return success ? NoContent() : NotFound();
    }


    [HttpPost]
    [Authorize(Roles = "fleet-admin")]
    public async Task<IActionResult> Create(CreateVehicleCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }
}

