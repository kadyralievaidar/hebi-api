using Hebi_Api.Features.Core.Extensions;
using Hebi_Api.Features.Diseases.Dtos;
using Hebi_Api.Features.Diseases.RequestHandling.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hebi_Api.Features.Diseases.Controllers;

[ApiController]
[Route("api/[controller]")]
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
    public async Task<IActionResult> Update([FromQuery] Guid appointmentId, [FromBody] CreateDiseaseDto dto, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new UpdateDiseaseRequest(appointmentId, dto), cancellationToken);
        return result.AsAspNetCoreResult();
    }

    [HttpDelete("id")]
    public async Task<IActionResult> Delete(Guid appointmentId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new DeleteDiseaseRequest(appointmentId), cancellationToken);
        return result.AsAspNetCoreResult();
    }

    [HttpGet("id")]
    public async Task<IActionResult> GetById(Guid appointmentId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetDiseaseByIdRequest(appointmentId), cancellationToken);
        return result.AsAspNetCoreResult();
    }

    [HttpGet]
    public async Task<IActionResult> GetAppointments([FromQuery]GetPagedListOfDiseaseDto dto, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetPagedListOfDiseaseRequest(dto), cancellationToken);
        return result.AsAspNetCoreResult();
    }
}
