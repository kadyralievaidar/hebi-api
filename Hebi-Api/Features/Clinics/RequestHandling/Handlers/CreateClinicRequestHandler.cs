using Hebi_Api.Features.Clinics.RequestHandling.Requests;
using Hebi_Api.Features.Clinics.Services;
using Hebi_Api.Features.Core.Common.RequestHandling;
using MediatR;

namespace Hebi_Api.Features.Clinics.RequestHandling.Handlers;

public class CreateClinicRequestHandler : IRequestHandler<CreateClinicRequest, Response>
{
    private readonly IClinicsService _service;
    private readonly ILogger<CreateClinicRequestHandler> _logger;

    public CreateClinicRequestHandler(IClinicsService service, ILogger<CreateClinicRequestHandler> logger)
    {
        _service = service;
        _logger = logger;
    }

    public async Task<Response> Handle(CreateClinicRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _service.CreateClinicAsync(request.CreateClinicDto);
            return Response.Ok(request.Id, result);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return Response.InternalServerError(request.Id, e);
        }
    }
}
