using Hebi_Api.Features.Core.Common.RequestHandling;
using Hebi_Api.Features.UserCards.RequestHandling.Requests;
using Hebi_Api.Features.UserCards.Services;
using MediatR;

namespace Hebi_Api.Features.UserCards.RequestHandling.Handlers;

public class UpdateUserCardRequestHandler : IRequestHandler<UpdateUserCardRequest, Response>
{
    private readonly IUserCardsService _service;
    private readonly ILogger<UpdateUserCardRequestHandler> _logger;

    public UpdateUserCardRequestHandler(IUserCardsService service, ILogger<UpdateUserCardRequestHandler> logger)
    {
        _service = service;
        _logger = logger;
    }

    public async Task<Response> Handle(UpdateUserCardRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _service.UpdateUserCard(request.UserCardId, request.Dto);
            return Response.Ok(request.Id, result);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            throw;
        }
    }
}
