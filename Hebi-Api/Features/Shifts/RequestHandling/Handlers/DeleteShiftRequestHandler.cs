using Hebi_Api.Features.Core.Common.RequestHandling;
using Hebi_Api.Features.Shifts.RequestHandling.Requests;
using Hebi_Api.Features.Shifts.Services;
using MediatR;

namespace Hebi_Api.Features.Shifts.RequestHandling.Handlers;

public class DeleteShiftRequestHandler : IRequestHandler<DeleteShiftRequest, Response>
{
    private readonly IShiftsService _service;
    private readonly ILogger<DeleteShiftRequestHandler> _logger;

    public DeleteShiftRequestHandler(IShiftsService service, ILogger<DeleteShiftRequestHandler> logger)
    {
        _service = service;
        _logger = logger;
    }

    public async Task<Response> Handle(DeleteShiftRequest request, CancellationToken cancellationToken)
    {
        try
        {
            await _service.DeleteShift(request.ShiftId);
            return Response.Ok(request.Id);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return Response.InternalServerError(request.Id, e);
        }
    }
}
