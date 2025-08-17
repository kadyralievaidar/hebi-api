using Hebi_Api.Features.Core.Common.RequestHandling;
using Hebi_Api.Features.Shifts.RequestHandling.Requests;
using Hebi_Api.Features.Shifts.Services;
using MediatR;

namespace Hebi_Api.Features.Shifts.RequestHandling.Handlers;

public class CreateShiftsWithShiftTemplateRequestHandler : IRequestHandler<CreateShiftsWithShiftTemplateRequest, Response>
{
    private readonly ILogger<CreateShiftsWithShiftTemplateRequestHandler> _logger;
    private readonly IShiftsService _service;

    public CreateShiftsWithShiftTemplateRequestHandler(ILogger<CreateShiftsWithShiftTemplateRequestHandler> logger, IShiftsService service)
    {
        _logger = logger;
        _service = service;
    }

    public async Task<Response> Handle(CreateShiftsWithShiftTemplateRequest request, CancellationToken cancellationToken)
    {
        try
        {
            await _service.CreateShiftsWithShiftTemplate(request.Dto);
            return Response.Ok(request.Id);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return Response.BadRequest(request.Id, e);
        }
    }
}
