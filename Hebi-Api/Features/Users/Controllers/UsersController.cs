using System.Security.Claims;
using Hebi_Api.Features.Core.Common;
using Hebi_Api.Features.Core.DataAccess.Models;
using Hebi_Api.Features.Users.RequestHandling.Requests;
using MediatR;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using static OpenIddict.Abstractions.OpenIddictConstants;

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
    public async Task<IActionResult> Register(Dtos.RegisterUserDto model)
    {
        return Ok(await _mediator.Send(new RegisterUserRequest(model)));
    }

    [HttpPost("~/connect/token"), Produces("application/json")]
    public async Task<IActionResult> Exchange()
    {
        var request = HttpContext.GetOpenIddictServerRequest();
        if (request.IsPasswordGrantType())
        {
            var application = await _applicationManager.FindByClientIdAsync(request.ClientId) ??
                throw new InvalidOperationException("The application cannot be found.");

            var identity = new ClaimsIdentity(TokenValidationParameters.DefaultAuthenticationType, Claims.Name, Claims.Role);
            var user = await _userManager.FindByNameAsync(request.Username);
            identity.SetClaim(Claims.Subject, await _applicationManager.GetClientIdAsync(application));
            identity.SetClaim(Claims.Name, await _applicationManager.GetDisplayNameAsync(application));
            identity.SetClaim(ClaimTypes.NameIdentifier, user.Id.ToString());
            identity.SetClaim(Consts.ClinicIdClaim, "9d0a942c-c95e-4b63-a079-82e3024e6308");

            identity.SetDestinations(static claim => claim.Type switch
            {
                Claims.Name when claim.Subject.HasScope(Scopes.Profile)
                    => [Destinations.AccessToken, Destinations.IdentityToken],

                _ => [Destinations.AccessToken]
            });

            return SignIn(new ClaimsPrincipal(identity), OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        throw new NotImplementedException("The specified grant is not implemented.");
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
