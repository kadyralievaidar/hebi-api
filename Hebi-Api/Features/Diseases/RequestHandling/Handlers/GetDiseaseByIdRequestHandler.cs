using Hebi_Api.Features.Core.Common.RequestHandling;
using Hebi_Api.Features.Diseases.RequestHandling.Requests;
using Hebi_Api.Features.Diseases.Services;
using MediatR;

namespace Hebi_Api.Features.Diseases.RequestHandling.Handlers;

public class GetDiseaseByIdRequestHandler : IRequestHandler<GetDiseaseByIdRequest, Response>
{
    private readonly IDiseaseService _service;
    private readonly ILogger<GetDiseaseByIdRequestHandler> _logger;

    public GetDiseaseByIdRequestHandler(IDiseaseService service, ILogger<GetDiseaseByIdRequestHandler> logger)
    {
        _service = service;
        _logger = logger;
    }

    public async Task<Response> Handle(GetDiseaseByIdRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _service.GetDiseaseAsync(request.DiseaseId);
            return Response.Ok(request.Id, result);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            throw;
        }
        throw new NotImplementedException();
    }
}
