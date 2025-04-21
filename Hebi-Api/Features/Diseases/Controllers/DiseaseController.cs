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
        var request = new CreateDiseaseRequest(dto);
        return Ok(await _mediator.Send(request, cancellationToken));
    }

    [HttpPut("update")]
    public async Task<IActionResult> Update([FromQuery] Guid appointmentId, [FromBody] CreateDiseaseDto dto, CancellationToken cancellationToken)
    {
        var request = new UpdateDiseaseRequest(appointmentId, dto);
        return Ok(await _mediator.Send(request, cancellationToken));
    }

    [HttpDelete("id")]
    public async Task<IActionResult> Delete(Guid appointmentId, CancellationToken cancellationToken)
    {
        var request = new DeleteDiseaseRequest(appointmentId);
        return Ok(await _mediator.Send(request, cancellationToken));
    }

    [HttpGet("id")]
    public async Task<IActionResult> GetById(Guid appointmentId, CancellationToken cancellationToken)
    {
        var request = new GetDiseaseByIdRequest(appointmentId);
        return Ok(await _mediator.Send(request, cancellationToken));
    }

    [HttpGet]
    public async Task<IActionResult> GetAppointments(GetPagedListOfDiseaseDto dto, CancellationToken cancellationToken)
    {
        var request = new GetPagedListOfDiseaseRequest(dto);
        return Ok(await _mediator.Send(request, cancellationToken));
    }
}
