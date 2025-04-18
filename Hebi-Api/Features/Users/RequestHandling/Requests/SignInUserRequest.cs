using Hebi_Api.Features.Core.Common.RequestHandling;
using Hebi_Api.Features.Users.Dtos;

namespace Hebi_Api.Features.Users.RequestHandling.Requests;

public class SignInUserRequest : Request<Response>
{
    public RegisterUserDto RegisterUserDto { get; set; }

    public SignInUserRequest(RegisterUserDto registerUserDto)
    {
        RegisterUserDto = registerUserDto;
    }
}
