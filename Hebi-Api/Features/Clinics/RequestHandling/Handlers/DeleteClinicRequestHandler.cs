using Hebi_Api.Features.Appointments.RequestHandling.Requests;
using Hebi_Api.Features.Clinics.Services;
using Hebi_Api.Features.Core.Common.RequestHandling;
using MediatR;

namespace Hebi_Api.Features.Clinics.RequestHandling.Handlers;

public class DeleteClinicRequestHandler : IRequestHandler<DeleteAppointmentRequest, Response>
{
    private readonly IClinicsService _service;
    private readonly ILogger<DeleteClinicRequestHandler> _looger;

    public DeleteClinicRequestHandler(IClinicsService service, ILogger<DeleteClinicRequestHandler> looger)
    {
        _service = service;
        _looger = looger;
    }

    public Task<Response> Handle(DeleteAppointmentRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
