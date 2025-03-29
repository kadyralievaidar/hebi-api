using Hebi_Api.Features.Clinics.Dtos;
using Hebi_Api.Features.Clinics.RequestHandling.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hebi_Api.Features.Clinics.Controllers;
public class ClinicController : ControllerBase
{
    private readonly IMediator _mediator;

    public ClinicController(IMediator mediator) => _mediator = mediator;

    [HttpPost("create-clinic")]
    public async Task<IActionResult> Create([FromBody] CreateClinicDto dto, CancellationToken cancellationToken)
    {
        var request = new CreateClinicRequest(dto);
        return Ok(await _mediator.Send(request, cancellationToken));
    }

    [HttpPut("update")]
    public async Task<IActionResult> Update([FromQuery] Guid appointmentId, [FromBody] CreateClinicDto dto, CancellationToken cancellationToken)
    {
        var request = new UpdateClinicRequest(appointmentId, dto);
        return Ok(await _mediator.Send(request, cancellationToken));
    }

    [HttpDelete("id")]
    public async Task<IActionResult> Delete(Guid appointmentId, CancellationToken cancellationToken)
    {
        var request = new DeleteClinicRequest(appointmentId);
        return Ok(await _mediator.Send(request, cancellationToken));
    }

    [HttpGet("id")]
    public async Task<IActionResult> GetById(Guid appointmentId, CancellationToken cancellationToken)
    {
        var request = new GetClinicByIdRequest(appointmentId);
        return Ok(await _mediator.Send(request, cancellationToken));
    }

    [HttpGet]
    public async Task<IActionResult> GetAppointments(GetPagedListOfClinicDto dto, CancellationToken cancellationToken)
    {
        var request = new GetPagedListClinicRequest(dto);
        return Ok(await _mediator.Send(request, cancellationToken));
    }
}
