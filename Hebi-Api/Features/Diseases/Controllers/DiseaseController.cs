using Hebi_Api.Features.Core.Extensions;
using Hebi_Api.Features.Diseases.Dtos;
using Hebi_Api.Features.Diseases.RequestHandling.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;

namespace Hebi_Api.Features.Diseases.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
public class DiseaseController : ControllerBase
{
    private readonly IMediator _mediator;

    public DiseaseController(IMediator mediator) => _mediator = mediator;

    [HttpPost("create-disease")]
    public async Task<IActionResult> Create([FromBody] CreateDiseaseDto dto, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CreateDiseaseRequest(dto),cancellationToken);
        return result.AsAspNetCoreResult();
    }

    [HttpPut("update")]
    public async Task<IActionResult> Update(Guid diseaseId, [FromBody] CreateDiseaseDto dto, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new UpdateDiseaseRequest(diseaseId, dto), cancellationToken);
        return result.AsAspNetCoreResult();
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(Guid diseaseId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new DeleteDiseaseRequest(diseaseId), cancellationToken);
        return result.AsAspNetCoreResult();
    }

    [HttpGet("id")]
    public async Task<IActionResult> GetById(Guid diseaseId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetDiseaseByIdRequest(diseaseId), cancellationToken);
        return result.AsAspNetCoreResult();
    }

    [HttpGet]
    public async Task<IActionResult> GetAppointments([FromQuery]GetPagedListOfDiseaseDto dto, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetPagedListOfDiseaseRequest(dto), cancellationToken);
        return result.AsAspNetCoreResult();
    }
}
