using Hebi_Api.Features.Clinics.RequestHandling.Requests;
using Hebi_Api.Features.Clinics.Services;
using Hebi_Api.Features.Core.Common.RequestHandling;
using MediatR;

namespace Hebi_Api.Features.Clinics.RequestHandling.Handlers;

public class RemoveDoctorRequestHandler : IRequestHandler<RemoveDoctorsRequest, Response>
{
    private readonly IClinicsService _service;
    private readonly ILogger<RemoveDoctorRequestHandler> _logger;

    public RemoveDoctorRequestHandler(IClinicsService service, ILogger<RemoveDoctorRequestHandler> logger)
    {
        _service = service;
        _logger = logger;
    }

    public async Task<Response> Handle(RemoveDoctorsRequest request, CancellationToken cancellationToken)
    {
        try
        {
            await _service.RemoveDoctorsFromClinic(request.DoctorIds);
            return Response.Ok(request.Id);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return Response.BadRequest(request.Id, e);
        }
    }
}
