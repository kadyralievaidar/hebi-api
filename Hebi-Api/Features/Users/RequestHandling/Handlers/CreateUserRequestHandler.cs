using Hebi_Api.Features.Core.Common.RequestHandling;
using Hebi_Api.Features.Users.RequestHandling.Requests;
using Hebi_Api.Features.Users.Services;
using MediatR;

namespace Hebi_Api.Features.Users.RequestHandling.Handlers;

public class CreateUserRequestHandler : IRequestHandler<CreateUserRequest, Response>
{
    private readonly IUsersService _service;
    private readonly ILogger<CreateUserRequestHandler> _logger;

    public CreateUserRequestHandler(IUsersService service, ILogger<CreateUserRequestHandler> logger)
    {
        _service = service;
        _logger = logger;
    }

    public async Task<Response> Handle(CreateUserRequest request, CancellationToken cancellationToken)
    {
        try
        {
            await _service.CreateUser(request.CreateUserDto);
            return Response.Ok(request.Id);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return Response.InternalServerError(request.Id, e);
        }
    }
}
