using Application.Abstractions.DTOs;
using Application.Abstractions.Interfaces;
using Application.Commands;
using Application.Queries;
using Microsoft.AspNetCore.Authorization;

namespace FinalLab.Controllers;
using Microsoft.AspNetCore.Mvc;

using MediatR;
using Microsoft.AspNetCore.Mvc;


[ApiController]
[Route("api/[controller]")]
public class DriversController : ControllerBase
{
    private readonly IMediator _mediator;
    public DriversController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    [Authorize(Roles = "fleet-user,fleet-admin")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetAllDriversQuery());
        return Ok(result);
    }
    [HttpGet("{id}")]
    [Authorize(Roles = "fleet-user,fleet-admin")]

    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetDriverByIdQuery(id));
        return result is null ? NotFound() : Ok(result);
    }
    
    [HttpPost]
    [Authorize(Roles = "fleet-admin")]

    public async Task<IActionResult> Create([FromBody] CreateDriverCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
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
        var success = await _mediator.Send(new DeleteDriverCommand(id));
        return success ? NoContent() : NotFound();
    }

    [HttpPost("assign-driver")]
    [Authorize(Roles = "fleet-admin")]

    public async Task<IActionResult> AssignDriver([FromBody] AssignDriverCommand command)
    {
        var success = await _mediator.Send(command);
        return success ? Ok("Driver assigned") : BadRequest("Assignment failed");
    }

}