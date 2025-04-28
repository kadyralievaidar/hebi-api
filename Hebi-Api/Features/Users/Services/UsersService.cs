using System.Security.Claims;
using Hebi_Api.Features.Core.DataAccess.Models;
using Hebi_Api.Features.Core.DataAccess.UOW;
using Hebi_Api.Features.Users.Dtos;
using Microsoft.AspNetCore.Identity;
using OpenIddict.Abstractions;

namespace Hebi_Api.Features.Users.Services;

public class UsersService : IUsersService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public UsersService(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor, 
        UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        _unitOfWork = unitOfWork;
        _contextAccessor = contextAccessor;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<ClaimsPrincipal> SignInUser(RegisterUserDto dto)
    {
        var user = await _userManager.FindByNameAsync(dto.UserName);
        if (user != null)
            return await _signInManager.CreateUserPrincipalAsync(user);

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
            principal.SetScopes(dto.Scopes);
            principal.SetResources("resource_server");
            await _signInManager.SignInAsync(user, true);
            return principal;
        }
        return new ClaimsPrincipal();
    }
}
