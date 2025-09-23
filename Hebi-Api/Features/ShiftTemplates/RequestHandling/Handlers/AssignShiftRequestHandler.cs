using Hebi_Api.Features.Core.Common.RequestHandling;
using Hebi_Api.Features.Shifts.RequestHandling.Requests;
using Hebi_Api.Features.Shifts.Services;
using MediatR;

namespace Hebi_Api.Features.ShiftTemplates.RequestHandling.Handlers;

public class AssignShiftRequestHandler : IRequestHandler<AssignShiftRequest, Response>
{
    private readonly ILogger<AssignShiftRequestHandler> _logger;
    private readonly IShiftsService _shiftService;

    public AssignShiftRequestHandler(ILogger<AssignShiftRequestHandler> logger, IShiftsService shiftService)
    {
        _logger = logger;
        _shiftService = shiftService;
    }

    public async Task<Response> Handle(AssignShiftRequest request, CancellationToken cancellationToken)
    {
        try
        {
            await _shiftService.AssignShift(request.DoctorId, request.ShiftId);
            return Response.Ok(request.Id);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return Response.BadRequest(request.Id, e);
        }
    }
}
