using Hebi_Api.Features.Core.Common.RequestHandling;
using Hebi_Api.Features.Shifts.RequestHandling.Requests;
using Hebi_Api.Features.Shifts.Services;
using MediatR;

namespace Hebi_Api.Features.Shifts.RequestHandling.Handlers;

public class GetByIdShiftRequestHandler : IRequestHandler<GetShiftByIdRequest, Response>
{
    private readonly IShiftsService _service;
    private readonly ILogger<GetByIdShiftRequestHandler> _logger;

    public GetByIdShiftRequestHandler(IShiftsService service, ILogger<GetByIdShiftRequestHandler> logger)
    {
        _service = service;
        _logger = logger;
    }

    public async Task<Response> Handle(GetShiftByIdRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _service.GetShiftAsync(request.ShiftId);
            return Response.Ok(request.Id, result);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return Response.InternalServerError(request.Id, e);
        }
    }
}
