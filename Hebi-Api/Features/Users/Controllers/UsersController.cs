using Hebi_Api.Features.Core.DataAccess.Models;
using Hebi_Api.Features.Users.Dtos;
using Hebi_Api.Features.Core.Extensions;
using Hebi_Api.Features.Users.RequestHandling.Requests;
using MediatR;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;

namespace Hebi_Api.Features.Users.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IOpenIddictApplicationManager _applicationManager;
    private readonly IMediator _mediator;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public UsersController(IMediator mediator,
        SignInManager<ApplicationUser> signInManager,
        IOpenIddictApplicationManager applicationManager,
        UserManager<ApplicationUser> userManager)
    {
        _mediator = mediator;
        _signInManager = signInManager;
        _applicationManager = applicationManager;
        _userManager = userManager;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserDto model)
    {
        var result = await _mediator.Send(new RegisterUserRequest(model));
        return result.AsAspNetCoreResult();
    }

    [HttpPost("~/connect/token"), Produces("application/json")]
    public async Task<IActionResult> Exchange()
    {
        var request = HttpContext.GetOpenIddictServerRequest();
        var result = await _mediator.Send(new TokenRequest(request));
        return SignIn(result.Principal, result.AuthScheme);
    }

    [HttpPost("~/connect/logout"), ValidateAntiForgeryToken]
    public async Task<IActionResult> LogoutPost()
    {
        await _signInManager.SignOutAsync();

        return SignOut(
            authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
            properties: new AuthenticationProperties
            {
                RedirectUri = "/"
            });
    }
}
