using Hebi_Api.Features.Core.Extensions;
using Hebi_Api.Features.Shifts.Dtos;
using Hebi_Api.Features.Shifts.RequestHandling.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hebi_Api.Features.UserCards.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserCardsController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserCardsController(IMediator mediator) => _mediator = mediator;

    [HttpDelete("id")]
    public async Task<IActionResult> Delete(Guid appointmentId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new DeleteShiftRequest(appointmentId), cancellationToken);
        return result.AsAspNetCoreResult();
    }

    [HttpGet("id")]
    public async Task<IActionResult> GetById(Guid appointmentId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetShiftByIdRequest(appointmentId), cancellationToken);
        return result.AsAspNetCoreResult();
    }

    [HttpGet]
    public async Task<IActionResult> GetAppointments(GetPagedListOfShiftsDto dto, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send( new GetPagedListOfShiftsRequest(dto), cancellationToken);
        return result.AsAspNetCoreResult();
    }
}
