using Hebi_Api.Features.Appointments.Dtos;
using Hebi_Api.Features.Appointments.RequestHandling.Requests;
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
        var request = new CreateAppointmentRequest(dto);
        return Ok(await _mediator.Send(request, cancellationToken));
    }

    [HttpPut("update")]
    public async Task<IActionResult> Update([FromQuery] Guid appointmentId, [FromBody] UpdateAppointmentDto dto, CancellationToken cancellationToken)
    {
        var request = new UpdateAppointmentRequest(appointmentId, dto);
        return Ok(await _mediator.Send(request, cancellationToken));
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
        var request = new GetAppoitmentByIdRequest(appointmentId);
        return Ok(await _mediator.Send(request, cancellationToken));
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAppointments([FromQuery]GetPagedListOfAppointmentDto dto, CancellationToken cancellationToken)
    {
        var request = new GetPagedListOfAppointmentRequest(dto);
        return Ok(await _mediator.Send(request, cancellationToken));
    }
}
