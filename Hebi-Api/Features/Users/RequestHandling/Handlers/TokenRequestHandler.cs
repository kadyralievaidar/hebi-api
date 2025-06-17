using Hebi_Api.Features.Core.Common.RequestHandling;
using Hebi_Api.Features.Users.Dtos;
using Hebi_Api.Features.Users.Services;
using MediatR;

namespace Hebi_Api.Features.Users.RequestHandling.Handlers;

public class TokenRequestHandler : IRequestHandler<TokenRequest, TokenResponse>
{
    private readonly IUsersService _userService;
    private readonly ILogger<TokenRequestHandler> _logger;

    public TokenRequestHandler(IUsersService userService, ILogger<TokenRequestHandler> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    public async Task<TokenResponse> Handle(TokenRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _userService.Token(request.Request, cancellationToken);
            return  response;
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            throw;
        }
    }
}
