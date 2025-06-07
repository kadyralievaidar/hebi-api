using Hebi_Api.Features.Core.Common;
using Hebi_Api.Features.Core.Common.Enums;
using Hebi_Api.Features.Core.DataAccess.Models;
using Hebi_Api.Features.Users.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using static OpenIddict.Abstractions.OpenIddictConstants;
using System.Security.Claims;
using static OpenIddict.Abstractions.OpenIddictConstants.JsonWebTokenTypes;

namespace Hebi_Api.Features.Users.Services;

public class UsersService : IUsersService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IOpenIddictApplicationManager _applicationManager;


    public UsersService(
        UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IOpenIddictApplicationManager openIddictApplicationManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _applicationManager = openIddictApplicationManager;
    }

    public async Task Register(RegisterUserDto dto)
    {
        var user = await _userManager.FindByNameAsync(dto.UserName);
        if (user != null)
            await _signInManager.SignInAsync(user, true);

        var newUser = new ApplicationUser()
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            UserName = dto.UserName,
            Email = dto.Email
        };
        var result = await _userManager.CreateAsync(newUser, dto.Password);
        if (result.Succeeded)
        {
            var principal = await _signInManager.CreateUserPrincipalAsync(newUser);
            await _signInManager.SignInAsync(newUser, true);
            await _userManager.AddToRoleAsync(newUser, UserRoles.Doctor.ToString());
        }
    }

    ///
    public async Task<TokenResponse> Token(OpenIddictRequest request, CancellationToken cancellationToken)
    {
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
            identity.SetScopes(request.GetScopes());
            identity.SetDestinations(static claim => claim.Type switch
            {
                Claims.Name when claim.Subject.HasScope(Scopes.Profile)
                    => [Destinations.AccessToken, Destinations.IdentityToken],

                _ => [Destinations.AccessToken]
            });
            
            var result = new TokenResponse() 
            {
                Principal = new ClaimsPrincipal(identity), 
                AuthScheme = OpenIddictServerAspNetCoreDefaults.AuthenticationScheme
            };
            return result;
        }
        return new TokenResponse();
    }
}
