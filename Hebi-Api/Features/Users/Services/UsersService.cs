using Hebi_Api.Features.Core.Common;
using Hebi_Api.Features.Core.Common.Enums;
using Hebi_Api.Features.Core.DataAccess.Models;
using Hebi_Api.Features.Core.DataAccess.UOW;
using Hebi_Api.Features.Users.Dtos;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using System.Security.Claims;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Hebi_Api.Features.Users.Services;

public class UsersService : IUsersService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IOpenIddictApplicationManager _applicationManager;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IUnitOfWork _unitOfWork;

    public UsersService(
        UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, 
        IOpenIddictApplicationManager openIddictApplicationManager, IHttpContextAccessor contextAccessor,
        IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _applicationManager = openIddictApplicationManager;
        _contextAccessor = contextAccessor;
        _unitOfWork = unitOfWork;
    }

    public async Task CreateUser(CreateUserDto dto)
    {
        var user = await _userManager.FindByNameAsync(dto.RegisterDto.UserName);
        if (user != null)
            await _signInManager.SignInAsync(user, true);

        var clinic = await _unitOfWork.ClinicRepository.FirstOrDefaultAsync(x => x.Id == dto.ClinicId);
        var newUser = new ApplicationUser()
        {
            FirstName = dto.RegisterDto.FirstName,
            LastName = dto.RegisterDto.LastName,
            UserName = dto.RegisterDto.UserName,
            Email = dto.RegisterDto.Email,
            PhoneNumber = dto.RegisterDto.PhoneNumber,
            ClinicId = clinic!.ClinicId,
            Clinic = clinic
        };
        var result = await _userManager.CreateAsync(newUser, dto.RegisterDto.Password);
        if (result.Succeeded)
        {
            await _signInManager.CreateUserPrincipalAsync(newUser);
            await _signInManager.SignInAsync(newUser, true);
            await _userManager.AddToRoleAsync(newUser, UserRoles.Doctor.ToString());
        }
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
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber
        };
        var result = await _userManager.CreateAsync(newUser, dto.Password);
        if (result.Succeeded)
        {
            await _signInManager.CreateUserPrincipalAsync(newUser);
            await _signInManager.SignInAsync(newUser, true);
            await _userManager.AddToRoleAsync(newUser, UserRoles.Admin.ToString());
        }
    }

    ///
    public async Task<TokenResponse> Token(OpenIddictRequest request, CancellationToken cancellationToken)
    {
        if (request.IsPasswordGrantType())
        {
            var application = await _applicationManager.FindByClientIdAsync(request.ClientId!) ??
                throw new InvalidOperationException("The application cannot be found.");

            var identity = await ConfigIdentity(request, application);

            var result = new TokenResponse() 
            {
                Principal = new ClaimsPrincipal(identity!), 
                AuthScheme = OpenIddictServerAspNetCoreDefaults.AuthenticationScheme
            };
            return result;
        }
        if(request.IsRefreshTokenGrantType()) 
        {
            var result = await _contextAccessor.HttpContext!.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

            if (!result.Succeeded)
            {
                throw new InvalidOperationException("The refresh token is no longer valid.");
            }

            var principal = result.Principal;

            var response = new TokenResponse()
            {
                Principal = principal!,
                AuthScheme = "Bearer"
            };
            return response;
        }
        return new TokenResponse();
    }
    private async Task<ClaimsIdentity?> ConfigIdentity(OpenIddictRequest request, object? application)
    {
        var user = await _unitOfWork.UsersRepository.FirstOrDefaultAsync(x => x.NormalizedUserName == request.Username!.ToUpperInvariant());
        var roles = await _userManager.GetRolesAsync(user);
        var identity = new ClaimsIdentity(TokenValidationParameters.DefaultAuthenticationType, Claims.Name, Claims.Role);

        identity.SetClaim(Claims.Subject, await _applicationManager.GetClientIdAsync(application!));
        identity.SetClaim(Claims.Name, await _applicationManager.GetDisplayNameAsync(application!));
        identity.SetClaim(ClaimTypes.NameIdentifier, user!.Id.ToString());
        identity.SetClaim(Consts.ClinicIdClaim, user.ClinicId.ToString());
        identity.SetClaim(Consts.Role, roles.First());

        identity.SetScopes(request.GetScopes());
        identity.SetDestinations(static claim => claim.Type switch
        {
            Claims.Name when claim.Subject!.HasScope(Scopes.Profile)
                => [Destinations.AccessToken],
            ClaimTypes.NameIdentifier => new[] { Destinations.AccessToken},

            Consts.ClinicIdClaim => new[] { Destinations.AccessToken },

            _ => [Destinations.AccessToken]
        });
        return identity;
    }
}
