using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using OpenIddict.Abstractions;
using Hebi_Api.Features.Users.RequestHandling.Requests;
using Hebi_Api.Features.Users.Dtos;

namespace Hebi_Api.Features.Users.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("~/connect/token")]
    public async Task<IActionResult> Exchange()
    {
        var request = HttpContext.GetOpenIddictServerRequest();
        var principal = await _mediator.Send(new SignInUserRequest(new RegisterUserDto()
        {
            UserName = request.Username,
            Password = request.Password,
            Scopes = request.GetScopes()
        }));
        return Ok(principal);
    }

}
