using Application.Commands;

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
    public async Task<IActionResult> GetAll() =>
        Ok(await _mediator.Send(new GetAllVehiclesQuery()));
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetDriverByIdQuery(id));
        return result is null ? NotFound() : Ok(result);
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateVehicleCommand command)
    {
        if (id != command.Id)
            return BadRequest("ID in route and body must match.");

        var success = await _mediator.Send(command);
        return success ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var success = await _mediator.Send(new DeleteVehicleCommand(id));
        return success ? NoContent() : NotFound();
    }


    [HttpPost]
    public async Task<IActionResult> Create(CreateVehicleCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }
}

