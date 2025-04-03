using Hebi_Api.Features.Core.Common.RequestHandling;
using Hebi_Api.Features.UserCards.RequestHandling.Requests;
using Hebi_Api.Features.UserCards.Services;
using MediatR;

namespace Hebi_Api.Features.UserCards.RequestHandling.Handlers;

public class CreateUserCardRequestHandler : IRequestHandler<CreateUserCardRequest, Response>
{
    private readonly IUserCardsService _userCardService;
    private readonly ILogger<CreateUserCardRequestHandler> _logger;

    public CreateUserCardRequestHandler(IUserCardsService userCardService, ILogger<CreateUserCardRequestHandler> logger)
    {
        _userCardService = userCardService;
        _logger = logger;
    }

    public async Task<Response> Handle(CreateUserCardRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var result = _userCardService.CreateUserCard(request.CreateUserCardDto);
            return Response.Ok(request.Id, result);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            throw;
        }
    }
}
