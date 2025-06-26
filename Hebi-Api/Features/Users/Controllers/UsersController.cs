using FluentValidation;
using Hebi_Api.Features.Core.Common.Enums;
using Hebi_Api.Features.Core.DataAccess.Models;
using Hebi_Api.Features.Core.Extensions;
using Hebi_Api.Features.Users.Dtos;
using Hebi_Api.Features.Users.RequestHandling.Requests;
using MediatR;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using OpenIddict.Validation.AspNetCore;

namespace Hebi_Api.Features.Users.Controllers;

/// <summary>
///     Users controller
/// </summary>
[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IOpenIddictTokenManager _tokenManager;

    public UsersController(IMediator mediator,
        SignInManager<ApplicationUser> signInManager,
        IOpenIddictTokenManager tokenManager)
    {
        _mediator = mediator;
        _signInManager = signInManager;
        _tokenManager = tokenManager;
    }

    /// <summary>
    ///     Register of user. User will be in admin role
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserDto model)
    {
        var result = await _mediator.Send(new RegisterUserRequest(model));
        return result.AsAspNetCoreResult();
    }

    /// <summary>
    ///     Receive a token 
    /// </summary>
    /// <param name="validator"></param>
    /// <returns></returns>
    [HttpPost("~/connect/token"), Produces("application/json")]
    public async Task<IActionResult> Exchange([FromServices] IValidator<TokenRequest> validator)
    {
        var openIdRequest = HttpContext.GetOpenIddictServerRequest();
        var request = new TokenRequest(openIdRequest);

        var validationResult = await validator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage });
            return BadRequest(new { Errors = errors });
        }

        var result = await _mediator.Send(request);
        return SignIn(result.Principal, result.AuthScheme);
    }

    /// <summary>
    ///     Sign out 
    /// </summary>
    /// <returns></returns>
    [HttpGet("~/connect/logout")]
    [Authorize(
    AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
    public async Task<IActionResult> LogoutPost()
    {
        return SignOut(new AuthenticationProperties
        {
            RedirectUri = "/"
        }, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }

    /// <summary>
    ///     Create doctor
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost("create-doctor")]
    [Authorize(
    AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
    public async Task<IActionResult> CreateDoctor([FromBody] CreateUserDto dto)
    {
        var result = await _mediator.Send(new CreateUserRequest(dto));
        return result.AsAspNetCoreResult();
    }

    /// <summary>
    ///     Create a patient
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost("create-patient")]
    [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
    public async Task<IActionResult> CreatePatient([FromBody] CreatePatientDto dto)
    {
        var result = await _mediator.Send(new CreatePatientRequest(dto));
        return result.AsAspNetCoreResult();
    }

    /// <summary>
    ///     Get user's info by id provided in route
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpGet("my-profile")]
    [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetUserById(Guid userId)
    {
        var result = await _mediator.Send(new GetUserByIdRequest(userId));
        return result.AsAspNetCoreResult();
    }
}
