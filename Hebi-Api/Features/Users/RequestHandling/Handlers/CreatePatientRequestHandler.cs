using Hebi_Api.Features.Core.Common.RequestHandling;
using Hebi_Api.Features.Users.RequestHandling.Requests;
using Hebi_Api.Features.Users.Services;
using MediatR;

namespace Hebi_Api.Features.Users.RequestHandling.Handlers;

public class CreatePatientRequestHandler : IRequestHandler<CreatePatientRequest, Response>
{
    private readonly IUsersService _service;
    private readonly ILogger<CreateUserRequestHandler> _logger;

    public CreatePatientRequestHandler(IUsersService service, ILogger<CreateUserRequestHandler> logger)
    {
        _service = service;
        _logger = logger;
    }

    public async Task<Response> Handle(CreatePatientRequest request, CancellationToken cancellationToken)
    {
        try
        {
            await _service.CreatePatient(request.Dto);
            return Response.Ok(request.Id);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return Response.InternalServerError(request.Id, e);
        }
    }
}
