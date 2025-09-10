using Hebi_Api.Features.Core.Common.RequestHandling;
using Hebi_Api.Features.Users.RequestHandling.Requests;
using Hebi_Api.Features.Users.Services;
using MediatR;

namespace Hebi_Api.Features.Users.RequestHandling.Handlers;

/// <summary>
///     Change user's password handler
/// </summary>
public class ChangePasswordHandler : IRequestHandler<ChangePasswordRequest, Response>
{
    private readonly ILogger<ChangePasswordHandler> _logger;
    private readonly IUsersService _service;

    public ChangePasswordHandler(ILogger<ChangePasswordHandler> logger, IUsersService service)
    {
        _logger = logger;
        _service = service;
    }

    public async Task<Response> Handle(ChangePasswordRequest request, CancellationToken cancellationToken)
    {
        try
        {
            await _service.ChangePassword(request.Dto);
            return Response.Ok(request.Id);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return Response.BadRequest(request.Id, e);
        }
    }
}
