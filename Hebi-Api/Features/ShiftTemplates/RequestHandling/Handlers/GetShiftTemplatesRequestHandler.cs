using Hebi_Api.Features.Core.Common.RequestHandling;
using Hebi_Api.Features.ShiftTemplates.RequestHandling.Requests;
using Hebi_Api.Features.ShiftTemplates.Services;
using MediatR;

namespace Hebi_Api.Features.ShiftTemplates.RequestHandling.Handlers;

public class GetShiftTemplatesRequestHandler : IRequestHandler<GetShiftTemplatesRequest, Response>
{
    private readonly ILogger<GetShiftTemplatesRequestHandler> _logger;
    private readonly IShiftTemplateService _service;

    public GetShiftTemplatesRequestHandler(ILogger<GetShiftTemplatesRequestHandler> logger, IShiftTemplateService service)
    {
        _logger = logger;
        _service = service;
    }

    public async Task<Response> Handle(GetShiftTemplatesRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _service.GetShiftTemplates(request.Dto);
            return Response.Ok(request.Id, result);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return Response.BadRequest(request.Id, e);
        }
    }
}
