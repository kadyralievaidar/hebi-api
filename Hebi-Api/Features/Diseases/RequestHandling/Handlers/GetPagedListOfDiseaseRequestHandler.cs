using Hebi_Api.Features.Core.Common.RequestHandling;
using Hebi_Api.Features.Diseases.RequestHandling.Requests;
using Hebi_Api.Features.Diseases.Services;
using MediatR;

namespace Hebi_Api.Features.Diseases.RequestHandling.Handlers;

public class GetPagedListOfDiseaseRequestHandler : IRequestHandler<GetPagedListOfDiseaseRequest, Response>
{
    private readonly IDiseaseService _service;
    private readonly ILogger<GetPagedListOfDiseaseRequestHandler> _logger;

    public GetPagedListOfDiseaseRequestHandler(IDiseaseService service, ILogger<GetPagedListOfDiseaseRequestHandler> logger)
    {
        _service = service;
        _logger = logger;
    }

    public async Task<Response> Handle(GetPagedListOfDiseaseRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _service.GetListOfDiseasesAsync(request.Dto);
            return Response.Ok(request.Id,result);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            throw;
        }
    }
}
