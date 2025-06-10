using Hebi_Api.Features.Clinics.RequestHandling.Requests;
using Hebi_Api.Features.Clinics.Services;
using Hebi_Api.Features.Core.Common.RequestHandling;
using MediatR;

namespace Hebi_Api.Features.Clinics.RequestHandling.Handlers;

public class GetClinicByIdRequestHandler : IRequestHandler<GetClinicByIdRequest, Response>
{
    private readonly IClinicsService _service;
    private readonly ILogger<GetClinicByIdRequestHandler> _logger;

    public GetClinicByIdRequestHandler(IClinicsService service, ILogger<GetClinicByIdRequestHandler> logger)
    {
        _service = service;
        _logger = logger;
    }

    public async Task<Response> Handle(GetClinicByIdRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var clinic = await _service.GetClinicAsync(request.ClinicId);
            return Response.Ok(request.Id, clinic);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            throw;
        }
    }
}
