using Hebi_Api.Features.Appointments.RequestHandling.Requests;
using Hebi_Api.Features.Clinics.Services;
using Hebi_Api.Features.Core.Common.RequestHandling;
using MediatR;

namespace Hebi_Api.Features.Clinics.RequestHandling.Handlers;

public class CreateClinicRequestHandler : IRequestHandler<CreateAppointmentRequest, Response>
{
    private readonly IClinicsService _service;
    private readonly ILogger<CreateClinicRequestHandler> _looger;

    public CreateClinicRequestHandler(IClinicsService service, ILogger<CreateClinicRequestHandler> looger)
    {
        _service = service;
        _looger = looger;
    }

    public Task<Response> Handle(CreateAppointmentRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
