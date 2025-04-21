using Hebi_Api.Features.Shifts.Dtos;
using Hebi_Api.Features.Shifts.RequestHandling.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hebi_Api.Features.Shifts.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShiftsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ShiftsController(IMediator mediator) => _mediator = mediator;

    [HttpPost("create-shift")]
    public async Task<IActionResult> Create([FromBody] CreateShiftDto dto, CancellationToken cancellationToken)
    {
        var request = new CreateShiftRequest(dto);
        return Ok(await _mediator.Send(request, cancellationToken));
    }

    [HttpPut("update")]
    public async Task<IActionResult> Update([FromQuery] Guid appointmentId, [FromBody] CreateShiftDto dto, CancellationToken cancellationToken)
    {
        var request = new UpdateShiftRequest(appointmentId, dto);
        return Ok(await _mediator.Send(request, cancellationToken));
    }

    [HttpDelete("id")]
    public async Task<IActionResult> Delete(Guid appointmentId, CancellationToken cancellationToken)
    {
        var request = new DeleteShiftRequest(appointmentId);
        return Ok(await _mediator.Send(request, cancellationToken));
    }

    [HttpGet("id")]
    public async Task<IActionResult> GetById(Guid appointmentId, CancellationToken cancellationToken)
    {
        var request = new GetShiftByIdRequest(appointmentId);
        return Ok(await _mediator.Send(request, cancellationToken));
    }

    [HttpGet]
    public async Task<IActionResult> GetAppointments(GetPagedListOfShiftsDto dto, CancellationToken cancellationToken)
    {
        var request = new GetPagedListOfShiftsRequest(dto);
        return Ok(await _mediator.Send(request, cancellationToken));
    }
}
