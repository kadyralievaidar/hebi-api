using Hebi_Api.Features.Clinics.RequestHandling.Requests;
using Hebi_Api.Features.Clinics.Services;
using Hebi_Api.Features.Core.Common.RequestHandling;
using MediatR;

namespace Hebi_Api.Features.Clinics.RequestHandling.Handlers;

public class DeleteClinicRequestHandler : IRequestHandler<DeleteClinicRequest, Response>
{
    private readonly IClinicsService _service;
    private readonly ILogger<DeleteClinicRequestHandler> _logger;

    public DeleteClinicRequestHandler(IClinicsService service, ILogger<DeleteClinicRequestHandler> logger)
    {
        _service = service;
        _logger = logger;
    }

    public async Task<Response> Handle(DeleteClinicRequest request, CancellationToken cancellationToken)
    {
        try
        {
            await _service.DeleteClinic(request.ClinicId);
            return Response.Ok(request.Id);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            throw;
        }
        throw new NotImplementedException();
    }
}
