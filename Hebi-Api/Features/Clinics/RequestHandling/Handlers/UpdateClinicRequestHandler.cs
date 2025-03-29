using Hebi_Api.Features.Clinics.RequestHandling.Requests;
using Hebi_Api.Features.Clinics.Services;
using Hebi_Api.Features.Core.Common.RequestHandling;
using MediatR;

namespace Hebi_Api.Features.Clinics.RequestHandling.Handlers;

public class UpdateClinicRequestHandler : IRequestHandler<UpdateClinicRequest, Response>
{
    private readonly IClinicsService _service;
    private readonly ILogger<UpdateClinicRequestHandler> _looger;

    public UpdateClinicRequestHandler(IClinicsService service, ILogger<UpdateClinicRequestHandler> looger)
    {
        _service = service;
        _looger = looger;
    }

    public Task<Response> Handle(UpdateClinicRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
