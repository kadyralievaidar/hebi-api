using Hebi_Api.Features.Appointments.RequestHandling.Requests;
using Hebi_Api.Features.Clinics.Services;
using Hebi_Api.Features.Core.Common.RequestHandling;
using MediatR;

namespace Hebi_Api.Features.Clinics.RequestHandling.Handlers;

public class GetPagedListClinicRequestHandler : IRequestHandler<GetPagedListOfAppointmentRequest, Response>
{
    private readonly IClinicsService _service;
    private readonly ILogger<GetPagedListClinicRequestHandler> _looger;

    public GetPagedListClinicRequestHandler(IClinicsService service, ILogger<GetPagedListClinicRequestHandler> looger)
    {
        _service = service;
        _looger = looger;
    }

    public Task<Response> Handle(GetPagedListOfAppointmentRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
