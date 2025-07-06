using Hebi_Api.Features.Core.Common.RequestHandling;
using Hebi_Api.Features.Users.RequestHandling.Requests;
using Hebi_Api.Features.Users.Services;
using MediatR;

namespace Hebi_Api.Features.Users.RequestHandling.Handlers;

public class ChangeUserInfoRequestHandler : IRequestHandler<ChangeUserInfoRequest, Response>
{
    private readonly ILogger<ChangeUserInfoRequestHandler> _logger;
    private readonly IUsersService _usersService;

    public ChangeUserInfoRequestHandler(ILogger<ChangeUserInfoRequestHandler> logger, IUsersService usersService)
    {
        _logger = logger;
        _usersService = usersService;
    }

    public async Task<Response> Handle(ChangeUserInfoRequest request, CancellationToken cancellationToken)
    {
        try
        {
            await _usersService.ChangeBasicInfo(request.Dto);
            return Response.Ok(request.Id);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return Response.BadRequest(request.Id, e);
        }
    }
}
