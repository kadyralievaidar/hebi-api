using Hebi_Api.Features.Appointments.Dtos;
using Hebi_Api.Features.Appointments.RequestHandling.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hebi_Api.Features.Appointments.Controllers;
public class AppointmentController : ControllerBase
{
    private readonly IMediator _mediator;

    public AppointmentController(IMediator mediator) => _mediator = mediator;

    [HttpPost("create-appoitment")]
    public async Task<IActionResult> Index([FromBody] CreateAppointmentDto dto)
    {
        var request = new CreateAppointmentRequest(dto);
        return Ok(await _mediator.Send(request));
    }
}
