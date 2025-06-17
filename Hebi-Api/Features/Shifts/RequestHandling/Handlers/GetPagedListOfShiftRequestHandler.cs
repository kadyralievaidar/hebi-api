using Hebi_Api.Features.Core.Common.RequestHandling;
using Hebi_Api.Features.Shifts.RequestHandling.Requests;
using Hebi_Api.Features.Shifts.Services;
using MediatR;

namespace Hebi_Api.Features.Shifts.RequestHandling.Handlers;

public class GetPagedListOfShiftRequestHandler : IRequestHandler<GetPagedListOfShiftsRequest, Response>
{
    private readonly IShiftsService _service;
    private readonly ILogger<GetPagedListOfShiftRequestHandler> _logger;

    public GetPagedListOfShiftRequestHandler(IShiftsService service, ILogger<GetPagedListOfShiftRequestHandler> logger)
    {
        _service = service;
        _logger = logger;
    }

    public async Task<Response> Handle(GetPagedListOfShiftsRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _service.GetListOfShiftsAsync(request.Dto);
            return Response.Ok(request.Id, result);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return Response.InternalServerError(request.Id, e);
        }
    }
}
