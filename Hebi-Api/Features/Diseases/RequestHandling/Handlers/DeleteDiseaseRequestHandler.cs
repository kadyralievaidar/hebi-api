using Hebi_Api.Features.Core.Common.RequestHandling;
using Hebi_Api.Features.Diseases.RequestHandling.Requests;
using Hebi_Api.Features.Diseases.Services;
using MediatR;

namespace Hebi_Api.Features.Diseases.RequestHandling.Handlers;

public class DeleteDiseaseRequestHandler : IRequestHandler<DeleteDiseaseRequest, Response>
{
    private readonly IDiseaseService _service;
    private readonly ILogger<DeleteDiseaseRequestHandler> _logger;

    public DeleteDiseaseRequestHandler(IDiseaseService service, ILogger<DeleteDiseaseRequestHandler> logger)
    {
        _service = service;
        _logger = logger;
    }

    public async Task<Response> Handle(DeleteDiseaseRequest request, CancellationToken cancellationToken)
    {
        try
        {
            await _service.DeleteDisease(request.DiseaseId);
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
