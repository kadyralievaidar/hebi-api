using Hebi_Api.Features.Core.Common.RequestHandling;
using Hebi_Api.Features.Users.RequestHandling.Requests;
using Hebi_Api.Features.Users.Services;
using MediatR;

namespace Hebi_Api.Features.Users.RequestHandling.Handlers;

public class SignInUserRequestHandler : IRequestHandler<SignInUserRequest, Response>
{
    private readonly IUsersService _service;
    private readonly ILogger<SignInUserRequestHandler> _logger;

    public SignInUserRequestHandler(IUsersService service, ILogger<SignInUserRequestHandler> logger)
    {
        _service = service;
        _logger = logger;
    }

    public async Task<Response> Handle(SignInUserRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _service.SignInUser(request.RegisterUserDto);
            return Response.Ok(request.Id, result);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            throw;
        }
    }
}
