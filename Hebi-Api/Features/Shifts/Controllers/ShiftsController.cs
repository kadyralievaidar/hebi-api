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
    public async Task<IActionResult> Update(Guid shiftId, [FromBody] CreateShiftDto dto, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new UpdateShiftRequest(shiftId, dto),cancellationToken);
        return result.AsAspNetCoreResult();
    }

    [HttpDelete("id")]
    public async Task<IActionResult> Delete(Guid shiftId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new DeleteShiftRequest(shiftId),cancellationToken);
        return result.AsAspNetCoreResult();
    }

    [HttpGet("id")]
    public async Task<IActionResult> GetById(Guid shiftId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetShiftByIdRequest(shiftId),cancellationToken);
        return result.AsAspNetCoreResult();
    }

    [HttpGet]
    public async Task<IActionResult> GetAppointments([FromQuery] GetPagedListOfShiftsDto dto, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetPagedListOfShiftsRequest(dto), cancellationToken);
        return result.AsAspNetCoreResult();
    }
}
