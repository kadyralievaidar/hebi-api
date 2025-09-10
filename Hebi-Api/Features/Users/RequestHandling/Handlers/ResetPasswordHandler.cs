using Hebi_Api.Features.Core.Common.RequestHandling;
using Hebi_Api.Features.Users.RequestHandling.Requests;
using Hebi_Api.Features.Users.Services;
using MediatR;

namespace Hebi_Api.Features.Users.RequestHandling.Handlers;

/// <summary>
///     Reset password handler
/// </summary>
public class ResetPasswordHandler : IRequestHandler<ResetPasswordRequest, Response>
{
    private readonly ILogger<ResetPasswordHandler> _logger;
    private readonly IUsersService _service;

    public ResetPasswordHandler(ILogger<ResetPasswordHandler> logger, IUsersService service)
    {
        _logger = logger;
        _service = service;
    }

    public async Task<Response> Handle(ResetPasswordRequest request, CancellationToken cancellationToken)
    {
        try
        {
            await _service.ResetPassword(request.Dto);
            return Response.Ok(request.Id);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return Response.BadRequest(request.Id, e);
        }
    }
}
