using Hebi_Api.Features.Appointments.Dtos;
using Hebi_Api.Features.Appointments.RequestHandling.Requests;
using Hebi_Api.Features.Core.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;

namespace Hebi_Api.Features.Appointments.Controllers;

[Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
[ApiController]
[Route("api/[controller]")]
public class AppointmentController : ControllerBase
{
    private readonly IMediator _mediator;
    public AppointmentController(IMediator mediator) => _mediator = mediator;

    [HttpPost("create-appoitment")]
    public async Task<IActionResult> Create([FromBody] CreateAppointmentDto dto, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CreateAppointmentRequest(dto), cancellationToken);
        return result.AsAspNetCoreResult();
    }

    [HttpPut("update")]
    public async Task<IActionResult> Update([FromQuery] Guid appointmentId, [FromBody] UpdateAppointmentDto dto, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new UpdateAppointmentRequest(appointmentId, dto));
        return result.AsAspNetCoreResult();
    }

    [HttpDelete("id")]
    public async Task<IActionResult> Delete(Guid appointmentId, CancellationToken cancellationToken)
    {
        var request = new DeleteAppointmentRequest(appointmentId);
        return Ok(await _mediator.Send(request, cancellationToken));
    }

    [HttpGet("id")]
    public async Task<IActionResult> GetById(Guid appointmentId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAppoitmentByIdRequest(appointmentId), cancellationToken);
        return result.AsAspNetCoreResult();
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAppointments([FromQuery]GetPagedListOfAppointmentDto dto, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetPagedListOfAppointmentRequest(dto), cancellationToken);
        return result.AsAspNetCoreResult();
    }
}
