using Hebi_Api.Features.Appointments.RequestHandling.Requests;
using Hebi_Api.Features.Appointments.Services;
using Hebi_Api.Features.Core.Common.RequestHandling;
using MediatR;

namespace Hebi_Api.Features.Appointments.RequestHandling.Handlers;

public class UpdateAppointmentRequestHandler : IRequestHandler<UpdateAppointmentRequest, Response>
{
    private readonly IAppointmentsService _service;
    private readonly ILogger<UpdateAppointmentRequestHandler> _logger;

    public UpdateAppointmentRequestHandler(IAppointmentsService service, ILogger<UpdateAppointmentRequestHandler> logger)
    {
        _service = service;
        _logger = logger;
    }

    public async Task<Response> Handle(UpdateAppointmentRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _service.UpdateAppointmentAsync(request.AppointmentId, request.Dto);
            return Response.Ok(request.Id, result);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            throw;
        }
    }
}
