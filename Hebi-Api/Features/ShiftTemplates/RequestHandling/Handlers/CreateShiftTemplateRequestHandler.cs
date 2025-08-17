using Hebi_Api.Features.Core.Common.RequestHandling;
using Hebi_Api.Features.ShiftTemplates.RequestHandling.Requests;
using Hebi_Api.Features.ShiftTemplates.Services;
using MediatR;

namespace Hebi_Api.Features.ShiftTemplates.RequestHandling.Handlers;

public class CreateShiftTemplateRequestHandler : IRequestHandler<CreateShiftTemplateRequest, Response>
{
    private readonly ILogger<CreateShiftTemplateRequestHandler> _logger;
    private readonly IShiftTemplateService _service;

    public CreateShiftTemplateRequestHandler(ILogger<CreateShiftTemplateRequestHandler> logger, IShiftTemplateService service)
    {
        _logger = logger;
        _service = service;
    }

    public async Task<Response> Handle(CreateShiftTemplateRequest request, CancellationToken cancellationToken)
    {
        try
        {
            await _service.CreateShiftTemplate(request.Dto);
            return Response.Ok(request.Id);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return Response.BadRequest(request.Id, e);
        }
    }
}
