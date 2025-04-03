using Hebi_Api.Features.Core.Common.RequestHandling;
using Hebi_Api.Features.UserCards.RequestHandling.Requests;
using Hebi_Api.Features.UserCards.Services;
using MediatR;

namespace Hebi_Api.Features.UserCards.RequestHandling.Handlers;

public class GetUserCardsPagedListRequestHandler : IRequestHandler<GetUserCardsPagedListRequest, Response>
{
    private readonly IUserCardsService _service;
    private readonly ILogger<GetUserCardsPagedListRequestHandler> _logger;

    public GetUserCardsPagedListRequestHandler(IUserCardsService service, ILogger<GetUserCardsPagedListRequestHandler> logger)
    {
        _service = service;
        _logger = logger;
    }

    public async Task<Response> Handle(GetUserCardsPagedListRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _service.GetListOfUserCardsAsync(request.Dto);
            return Response.Ok(request.Id, result);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            throw;
        }
        throw new NotImplementedException();
    }
}
