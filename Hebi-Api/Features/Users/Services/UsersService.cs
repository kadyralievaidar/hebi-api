using Hebi_Api.Features.Core.Common.Enums;
using Hebi_Api.Features.Core.DataAccess.Models;
using Hebi_Api.Features.Users.Dtos;
using Microsoft.AspNetCore.Identity;

namespace Hebi_Api.Features.Users.Services;

public class UsersService : IUsersService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public UsersService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
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
}
