using Hebi_Api.Features.Core.Extensions;
using Hebi_Api.Features.ShiftTemplates.Dtos;
using Hebi_Api.Features.ShiftTemplates.RequestHandling.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;

namespace Hebi_Api.Features.ShiftTemplates.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
public class ShiftTemplateController : ControllerBase
{
    private readonly IMediator _mediator;

    public ShiftTemplateController(IMediator mediator) => _mediator = mediator;
    /// <summary>
    ///     Create a single shift
    /// </summary>
    /// <param name="dto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("create-shift-template")]
    public async Task<IActionResult> Create([FromBody] CreateShiftTemplateDto dto, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CreateShiftTemplateRequest(dto), cancellationToken);
        return result.AsAspNetCoreResult();
    }

    /// <summary>
    ///     Update shift 
    /// </summary>
    /// <param name="shiftTemplateId"></param>
    /// <param name="dto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPut("update")]
    public async Task<IActionResult> 
        Update(Guid shiftTemplateId, [FromBody] CreateShiftTemplateDto dto, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new UpdateShiftTemplateRequest(shiftTemplateId, dto), cancellationToken);
        return result.AsAspNetCoreResult();
    }

    /// <summary>
    ///     Delete shift by id
    /// </summary>
    /// <param name="shiftTemplateId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpDelete("id")]
    public async Task<IActionResult> Delete(Guid shiftTemplateId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new DeleteShiftTemplateRequest(shiftTemplateId), cancellationToken);
        return result.AsAspNetCoreResult();
    }

    /// <summary>
    ///     Get shift template by id
    /// </summary>
    /// <param name="shiftTemplateId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("id")]
    public async Task<IActionResult> GetById(Guid shiftTemplateId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetShiftTemplateByIdRequest(shiftTemplateId), cancellationToken);
        return result.AsAspNetCoreResult();
    }

    /// <summary>
    ///     Get shift templates
    /// </summary>
    /// <param name="dto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> 
        GetShifts([FromQuery] GetPagedListOfShiftsTemplatesDto dto, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetShiftTemplatesRequest(dto), cancellationToken);
        return result.AsAspNetCoreResult();
    }
}
