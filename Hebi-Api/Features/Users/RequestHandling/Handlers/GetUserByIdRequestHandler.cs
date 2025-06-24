using Hebi_Api.Features.Core.Common.RequestHandling;
using Hebi_Api.Features.Users.RequestHandling.Requests;
using Hebi_Api.Features.Users.Services;
using MediatR;

namespace Hebi_Api.Features.Users.RequestHandling.Handlers;

public class GetUserByIdRequestHandler : IRequestHandler<GetUserByIdRequest, Response>
{
    private readonly IUsersService _service;
    private readonly ILogger<GetUserByIdRequestHandler> _logger;

    public GetUserByIdRequestHandler(IUsersService service, ILogger<GetUserByIdRequestHandler> logger)
    {
        _service = service;
        _logger = logger;
    }

    public async Task<Response> Handle(GetUserByIdRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _service.GetUserById(request.UserId);
            return Response.Ok(request.Id,result);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return Response.InternalServerError(request.Id, e);
        }
    }
}
