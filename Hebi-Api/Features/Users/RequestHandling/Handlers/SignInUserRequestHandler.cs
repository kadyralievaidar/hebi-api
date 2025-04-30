using Hebi_Api.Features.Core.Common.RequestHandling;
using Hebi_Api.Features.Users.RequestHandling.Requests;
using Hebi_Api.Features.Users.Services;
using MediatR;

namespace Hebi_Api.Features.Users.RequestHandling.Handlers;

public class SignInUserRequestHandler : IRequestHandler<RegisterUserRequest, Response>
{
    private readonly IUsersService _service;
    private readonly ILogger<SignInUserRequestHandler> _logger;

    public SignInUserRequestHandler(IUsersService service, ILogger<SignInUserRequestHandler> logger)
    {
        _service = service;
        _logger = logger;
    }

    public async Task<Response> Handle(RegisterUserRequest request, CancellationToken cancellationToken)
    {
        try
        {
            await _service.Register(request.RegisterUserDto);
            return Response.Ok(request.Id);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            throw;
        }
    }
}
