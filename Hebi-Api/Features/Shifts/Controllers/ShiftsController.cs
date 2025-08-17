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

    /// <summary>
    ///     Create a single shift
    /// </summary>
    /// <param name="dto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("create-shift")]
    public async Task<IActionResult> Create([FromBody] CreateShiftDto dto, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CreateShiftRequest(dto), cancellationToken);
        return result.AsAspNetCoreResult();
    }

    /// <summary>
    ///     Update shift 
    /// </summary>
    /// <param name="shiftId"></param>
    /// <param name="dto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPut("update")]
    public async Task<IActionResult> Update(Guid shiftId, [FromBody] CreateShiftDto dto, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new UpdateShiftRequest(shiftId, dto),cancellationToken);
        return result.AsAspNetCoreResult();
    }

    /// <summary>
    ///     Delete shift by id
    /// </summary>
    /// <param name="shiftId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpDelete("id")]
    public async Task<IActionResult> Delete(Guid shiftId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new DeleteShiftRequest(shiftId),cancellationToken);
        return result.AsAspNetCoreResult();
    }

    /// <summary>
    ///     Get shift by id
    /// </summary>
    /// <param name="shiftId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("id")]
    public async Task<IActionResult> GetById(Guid shiftId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetShiftByIdRequest(shiftId),cancellationToken);
        return result.AsAspNetCoreResult();
    }

    /// <summary>
    ///     Get shifts
    /// </summary>
    /// <param name="dto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> GetShifts([FromQuery] GetPagedListOfShiftsDto dto, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetPagedListOfShiftsRequest(dto), cancellationToken);
        return result.AsAspNetCoreResult();
    }

    /// <summary>
    ///     Generates shift based on shift template
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost("generate-shifts")]
    public async Task<IActionResult> GenerateShifts([FromBody] CreateShiftsWithTemplateDto dto)
    {
        var result = await _mediator.Send(new CreateShiftsWithShiftTemplateRequest(dto));
        return result.AsAspNetCoreResult();
    }
}
