using Hebi_Api.Features.Core.Common.RequestHandling;
using Hebi_Api.Features.Users.Dtos;

namespace Hebi_Api.Features.Users.RequestHandling.Requests;

/// <summary>
///     Request for creating user for clinic
/// </summary>
public class CreateUserRequest : Request<Response>
{
    /// <summary>
    ///     Create user dto
    /// </summary>
    public CreateUserDto CreateUserDto { get; set; }

    /// <summary>
    ///     ctr
    /// </summary>
    /// <param name="createUserDto"></param>
    public CreateUserRequest(CreateUserDto createUserDto)
    {
        CreateUserDto = createUserDto;
    }
}
