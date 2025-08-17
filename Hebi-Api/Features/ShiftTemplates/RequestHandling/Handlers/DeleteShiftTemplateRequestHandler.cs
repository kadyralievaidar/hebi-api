using Hebi_Api.Features.Core.Common.RequestHandling;
using Hebi_Api.Features.ShiftTemplates.RequestHandling.Requests;
using Hebi_Api.Features.ShiftTemplates.Services;
using MediatR;

namespace Hebi_Api.Features.ShiftTemplates.RequestHandling.Handlers;

public class DeleteShiftTemplateRequestHandler : IRequestHandler<DeleteShiftTemplateRequest, Response>
{
    private readonly ILogger<DeleteShiftTemplateRequestHandler> _logger;
    private readonly IShiftTemplateService _service;

    public DeleteShiftTemplateRequestHandler(ILogger<DeleteShiftTemplateRequestHandler> logger, IShiftTemplateService service)
    {
        _logger = logger;
        _service = service;
    }

    public async Task<Response> Handle(DeleteShiftTemplateRequest request, CancellationToken cancellationToken)
    {
        try
        {
            await _service.DeleteShiftTemplate(request.ShiftTemplateId);
            return Response.Ok(request.Id);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return Response.BadRequest(request.Id, e);
        }
    }
}
