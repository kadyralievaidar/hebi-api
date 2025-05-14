using Hebi_Api.Features.Clinics.Dtos;
using Hebi_Api.Features.Clinics.RequestHandling.Requests;
using Hebi_Api.Features.Core.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hebi_Api.Features.Clinics.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClinicController : ControllerBase
{
    private readonly IMediator _mediator;

    public ClinicController(IMediator mediator) => _mediator = mediator;

    [HttpPost("create-clinic")]
    public async Task<IActionResult> Create([FromBody] CreateClinicDto dto, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CreateClinicRequest(dto), cancellationToken);
        return result.AsAspNetCoreResult();
    }

    [HttpPut("update")]
    public async Task<IActionResult> Update([FromQuery] Guid appointmentId, [FromBody] CreateClinicDto dto, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new UpdateClinicRequest(appointmentId, dto), cancellationToken);
        return result.AsAspNetCoreResult();
    }

    [HttpDelete("id")]
    public async Task<IActionResult> Delete(Guid appointmentId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new DeleteClinicRequest(appointmentId),cancellationToken);
        return result.AsAspNetCoreResult();
    }

    [HttpGet("id")]
    public async Task<IActionResult> GetById(Guid appointmentId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetClinicByIdRequest(appointmentId), cancellationToken);
        return result.AsAspNetCoreResult();
    }

    [HttpGet]
    public async Task<IActionResult> GetAppointments(GetPagedListOfClinicDto dto, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetPagedListClinicRequest(dto), cancellationToken);
        return result.AsAspNetCoreResult();
    }
}
