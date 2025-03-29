using Hebi_Api.Features.Appointments.RequestHandling.Requests;
using Hebi_Api.Features.Appointments.Services;
using Hebi_Api.Features.Core.Common.RequestHandling;
using MediatR;

namespace Hebi_Api.Features.Appointments.RequestHandling.Handlers;

public class DeleteAppointmentRequestHandler : IRequestHandler<DeleteAppointmentRequest, Response>
{
    private readonly IAppointmentsService _appoitmentsService;
    private readonly ILogger<CreateAppointmentRequestHandler> _logger;
    public Task<Response> Handle(DeleteAppointmentRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
