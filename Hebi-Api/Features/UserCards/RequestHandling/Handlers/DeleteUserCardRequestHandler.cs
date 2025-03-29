using Hebi_Api.Features.Core.Common.RequestHandling;
using Hebi_Api.Features.UserCards.RequestHandling.Requests;
using Hebi_Api.Features.UserCards.Services;
using MediatR;

namespace Hebi_Api.Features.UserCards.RequestHandling.Handlers;

public class DeleteUserCardRequestHandler : IRequestHandler<DeleteUserCardRequest, Response>
{
    private readonly IUserCardsService _service;
    private readonly ILogger<DeleteUserCardRequestHandler> _logger;

    public DeleteUserCardRequestHandler(IUserCardsService service, ILogger<DeleteUserCardRequestHandler> logger)
    {
        _service = service;
        _logger = logger;
    }

    public Task<Response> Handle(DeleteUserCardRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
