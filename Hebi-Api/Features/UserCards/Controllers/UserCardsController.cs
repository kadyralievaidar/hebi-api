using Hebi_Api.Features.Core.Extensions;
using Hebi_Api.Features.UserCards.Dtos;
using Hebi_Api.Features.UserCards.RequestHandling.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;

namespace Hebi_Api.Features.UserCards.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
public class UserCardsController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserCardsController(IMediator mediator) => _mediator = mediator;

    [HttpDelete("id")]
    public async Task<IActionResult> Delete(Guid userCardId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new DeleteUserCardRequest(userCardId), cancellationToken);
        return result.AsAspNetCoreResult();
    }

    [HttpGet("id")]
    public async Task<IActionResult> GetById(Guid userCardId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetUserCardByIdRequest(userCardId), cancellationToken);
        return result.AsAspNetCoreResult();
    }

    [HttpGet]
    public async Task<IActionResult> GetUserCards([FromQuery]GetPagedListOfUserCardDto dto, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetUserCardsPagedListRequest(dto), cancellationToken);
        return result.AsAspNetCoreResult();
    }
}
