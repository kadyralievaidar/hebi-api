using Hebi_Api.Features.Core.Common.RequestHandling;
using Hebi_Api.Features.Users.RequestHandling.Requests;
using Hebi_Api.Features.Users.Services;
using MediatR;

namespace Hebi_Api.Features.Users.RequestHandling.Handlers;

public class ChangeUserRoleRequestHandler : IRequestHandler<ChangeUserRoleRequest, Response>
{
    private readonly ILogger<ChangeUserRoleRequestHandler> _logger;
    private readonly IUsersService _usersService;

    public ChangeUserRoleRequestHandler(ILogger<ChangeUserRoleRequestHandler> logger, IUsersService usersService)
    {
        _logger = logger;
        _usersService = usersService;
    }

    public async Task<Response> Handle(ChangeUserRoleRequest request, CancellationToken cancellationToken)
    {
        try
        {
            await _usersService.ChangeUserRole(request.Id, request.RoleName);
            return Response.Ok(request.Id);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return Response.BadRequest(request.Id, e);
        }
    }
}
