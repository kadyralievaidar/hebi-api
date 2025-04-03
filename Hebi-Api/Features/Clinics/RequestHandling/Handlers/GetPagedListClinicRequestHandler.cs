using Hebi_Api.Features.Clinics.RequestHandling.Requests;
using Hebi_Api.Features.Clinics.Services;
using Hebi_Api.Features.Core.Common.RequestHandling;
using MediatR;

namespace Hebi_Api.Features.Clinics.RequestHandling.Handlers;

public class GetPagedListClinicRequestHandler : IRequestHandler<GetPagedListClinicRequest, Response>
{
    private readonly IClinicsService _service;
    private readonly ILogger<GetPagedListClinicRequestHandler> _logger;

    public GetPagedListClinicRequestHandler(IClinicsService service, ILogger<GetPagedListClinicRequestHandler> logger)
    {
        _service = service;
        _logger = logger;
    }

    public async Task<Response> Handle(GetPagedListClinicRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var clinics = await _service.GetListOfClinicsAsync(request.Dto);
            return Response.Ok(request.Id, clinics);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            throw;
        }
    }
}
