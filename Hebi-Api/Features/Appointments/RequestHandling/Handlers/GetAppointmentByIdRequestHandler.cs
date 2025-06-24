using Hebi_Api.Features.Appointments.RequestHandling.Requests;
using Hebi_Api.Features.Appointments.Services;
using Hebi_Api.Features.Core.Common.RequestHandling;
using MediatR;

namespace Hebi_Api.Features.Appointments.RequestHandling.Handlers;

public class GetAppointmentByIdRequestHandler : IRequestHandler<GetAppoitmentByIdRequest, Response>
{
    private readonly IAppointmentsService _appoitmentsService;
    private readonly ILogger<CreateAppointmentRequestHandler> _logger;

    public GetAppointmentByIdRequestHandler(IAppointmentsService appoitmentsService, ILogger<CreateAppointmentRequestHandler> logger)
    {
        _appoitmentsService = appoitmentsService;
        _logger = logger;
    }

    public async Task<Response> Handle(GetAppoitmentByIdRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var appointment = await _appoitmentsService.GetAppointmentAsync(request.AppointmnetId);
            return Response.Ok(request.Id, appointment);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return Response.InternalServerError(request.Id, e);
        }
    }
}
