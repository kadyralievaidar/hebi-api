using Hebi_Api.Features.Core.Common.RequestHandling;
using Hebi_Api.Features.Shifts.RequestHandling.Requests;
using Hebi_Api.Features.Shifts.Services;
using MediatR;

namespace Hebi_Api.Features.Shifts.RequestHandling.Handlers;

public class UpdateShiftRequestHandler : IRequestHandler<UpdateShiftRequest, Response>
{
    private readonly IShiftsService _service;
    private readonly ILogger<UpdateShiftRequestHandler> _logger;

    public UpdateShiftRequestHandler(IShiftsService service, ILogger<UpdateShiftRequestHandler> logger)
    {
        _service = service;
        _logger = logger;
    }

    public Task<Response> Handle(UpdateShiftRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
