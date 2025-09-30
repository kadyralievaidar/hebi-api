using Hebi_Api.Features.Appointments.RequestHandling.Requests;
using Hebi_Api.Features.Appointments.Services;
using Hebi_Api.Features.Core.Common.RequestHandling;
using MediatR;

namespace Hebi_Api.Features.Appointments.RequestHandling.Handlers;

public class GenerateAppointmentsRequestHandler(ILogger<GenerateAppointmentsRequestHandler> logger, IAppointmentsService service) : IRequestHandler<GenerateAppointmentsRequest, Response>
{
    public async Task<Response> Handle(GenerateAppointmentsRequest request, CancellationToken cancellationToken)
    {
        try
        {
            await service.GenerateAppointments(request.Dto);
            return Response.Ok(request.Id);
        }
        catch (Exception e)
        {
            return Response.BadRequest(request.Id, e);
        }
    }
}
