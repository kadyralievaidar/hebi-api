using Hebi_Api.Features.Core.Extensions;
using Hebi_Api.Features.Shifts.Dtos;
using Hebi_Api.Features.Shifts.RequestHandling.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;

namespace Hebi_Api.Features.Shifts.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
public class ShiftsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ShiftsController(IMediator mediator) => _mediator = mediator;

    [HttpPost("create-shift")]
    public async Task<IActionResult> Create([FromBody] CreateShiftDto dto, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CreateShiftRequest(dto), cancellationToken);
        return result.AsAspNetCoreResult();
    }

    [HttpPut("update")]
    public async Task<IActionResult> Update([FromQuery] Guid appointmentId, [FromBody] CreateShiftDto dto, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new UpdateShiftRequest(appointmentId, dto),cancellationToken);
        return result.AsAspNetCoreResult();
    }

    [HttpDelete("id")]
    public async Task<IActionResult> Delete(Guid appointmentId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new DeleteShiftRequest(appointmentId),cancellationToken);
        return result.AsAspNetCoreResult();
    }

    [HttpGet("id")]
    public async Task<IActionResult> GetById(Guid appointmentId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetShiftByIdRequest(appointmentId),cancellationToken);
        return result.AsAspNetCoreResult();
    }

    [HttpGet]
    public async Task<IActionResult> GetAppointments(GetPagedListOfShiftsDto dto, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetPagedListOfShiftsRequest(dto), cancellationToken);
        return result.AsAspNetCoreResult();
    }
}
