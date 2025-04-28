using Hebi_Api.Features.Users.Dtos;
using System.Security.Claims;

namespace Hebi_Api.Features.Users.Services;

public interface IUsersService
{
    Task<ClaimsPrincipal> SignInUser(RegisterUserDto dto);
}

