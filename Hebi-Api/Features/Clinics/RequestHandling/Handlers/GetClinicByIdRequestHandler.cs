using Hebi_Api.Features.Appointments.RequestHandling.Requests;
using Hebi_Api.Features.Clinics.Services;
using Hebi_Api.Features.Core.Common.RequestHandling;
using MediatR;

namespace Hebi_Api.Features.Clinics.RequestHandling.Handlers;

public class GetClinicByIdRequestHandler : IRequestHandler<DeleteAppointmentRequest, Response>
{
    private readonly IClinicsService _service;
    private readonly ILogger<GetClinicByIdRequestHandler> _looger;

    public GetClinicByIdRequestHandler(IClinicsService service, ILogger<GetClinicByIdRequestHandler> looger)
    {
        _service = service;
        _looger = looger;
    }

    public Task<Response> Handle(DeleteAppointmentRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
