using Hebi_Api.Features.Core.Common.RequestHandling;
using Hebi_Api.Features.Diseases.RequestHandling.Requests;
using Hebi_Api.Features.Diseases.Services;
using MediatR;

namespace Hebi_Api.Features.Diseases.RequestHandling.Handlers;

public class CreateDiseaseRequestHandler : IRequestHandler<CreateDiseaseRequest, Response>
{
    private readonly IDiseaseService _service;
    private readonly ILogger<CreateDiseaseRequestHandler> _logger;

    public CreateDiseaseRequestHandler(IDiseaseService service, ILogger<CreateDiseaseRequestHandler> logger)
    {
        _service = service;
        _logger = logger;
    }

    public async Task<Response> Handle(CreateDiseaseRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _service.CreateDisease(request.CreateDiseaseDto);
            return Response.Ok(request.Id, result);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            throw;
        }
    }
}
