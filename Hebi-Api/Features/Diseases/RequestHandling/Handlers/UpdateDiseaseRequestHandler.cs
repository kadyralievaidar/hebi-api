using Hebi_Api.Features.Core.Common.RequestHandling;
using Hebi_Api.Features.Diseases.RequestHandling.Requests;
using Hebi_Api.Features.Diseases.Services;
using MediatR;

namespace Hebi_Api.Features.Diseases.RequestHandling.Handlers;

public class UpdateDiseaseRequestHandler : IRequestHandler<UpdateDiseaseRequest, Response>
{
    private readonly IDiseaseService _service;
    private readonly ILogger<UpdateDiseaseRequestHandler> _logger;

    public UpdateDiseaseRequestHandler(IDiseaseService service, ILogger<UpdateDiseaseRequestHandler> logger)
    {
        _service = service;
        _logger = logger;
    }

    public async Task<Response> Handle(UpdateDiseaseRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _service.UpdateDisease(request.DiseaseId, request.CreateDiseaseDto);
            return Response.Ok(request.Id, result);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            throw;
        }
    }
}
