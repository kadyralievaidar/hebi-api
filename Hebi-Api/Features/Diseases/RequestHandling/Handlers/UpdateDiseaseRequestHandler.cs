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

    public Task<Response> Handle(UpdateDiseaseRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
