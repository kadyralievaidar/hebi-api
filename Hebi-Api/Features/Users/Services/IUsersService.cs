using Hebi_Api.Features.Users.Dtos;
using OpenIddict.Abstractions;

namespace Hebi_Api.Features.Users.Services;

public interface IUsersService
{
    /// <summary>
    ///     Register user
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task Register(RegisterUserDto dto);

    /// <summary>
    ///     Get token for user
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<TokenResponse> Token(OpenIddictRequest request, CancellationToken cancellationToken);
}

