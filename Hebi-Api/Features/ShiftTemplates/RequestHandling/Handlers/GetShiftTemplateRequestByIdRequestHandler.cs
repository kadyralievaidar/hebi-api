using Hebi_Api.Features.Core.Common.RequestHandling;
using Hebi_Api.Features.ShiftTemplates.RequestHandling.Requests;
using Hebi_Api.Features.ShiftTemplates.Services;
using MediatR;

namespace Hebi_Api.Features.ShiftTemplates.RequestHandling.Handlers;

public class GetShiftTemplateRequestByIdRequestHandler : IRequestHandler<GetShiftTemplateByIdRequest, Response>
{
    private readonly ILogger<GetShiftTemplateRequestByIdRequestHandler> _logger;
    private readonly IShiftTemplateService _service;

    public GetShiftTemplateRequestByIdRequestHandler(ILogger<GetShiftTemplateRequestByIdRequestHandler> logger, IShiftTemplateService service)
    {
        _logger = logger;
        _service = service;
    }

    public async Task<Response> Handle(GetShiftTemplateByIdRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _service.GetShiftTemplateById(request.ShiftTemplateId);
            return Response.Ok(request.Id, result);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return Response.BadRequest(request.Id, e);
        }
    }
}
