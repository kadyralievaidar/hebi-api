using Hebi_Api.Features.Users.Dtos;

namespace Hebi_Api.Features.Users.Services;

public interface IUsersService
{
    Task Register(RegisterUserDto dto);
}

