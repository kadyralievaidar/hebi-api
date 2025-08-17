using Hebi_Api.Features.Core.Common.RequestHandling;
using Hebi_Api.Features.ShiftTemplates.RequestHandling.Requests;
using Hebi_Api.Features.ShiftTemplates.Services;
using MediatR;

namespace Hebi_Api.Features.ShiftTemplates.RequestHandling.Handlers;

/// <summary>
///     Update shift template request handler
/// </summary>
public class UpdateShiftTemplateRequestHandler : IRequestHandler<UpdateShiftTemplateRequest, Response>
{
    private readonly ILogger<UpdateShiftTemplateRequestHandler> _logger;
    private readonly IShiftTemplateService _service;

    public UpdateShiftTemplateRequestHandler(ILogger<UpdateShiftTemplateRequestHandler> logger, IShiftTemplateService service)
    {
        _logger = logger;
        _service = service;
    }

    public async Task<Response> Handle(UpdateShiftTemplateRequest request, CancellationToken cancellationToken)
    {
        try
        {
            await _service.UpdateShiftTempalate(request.ShiftTemplateId, request.Dto);
            return Response.Ok(request.Id);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return Response.BadRequest(request.Id, e);
        }
    }
}
