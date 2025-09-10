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

    /// <summary>
    ///     Create a user with clinic Id
    /// </summary>
    /// <returns></returns>
    Task CreateUser(CreateUserDto dto);

    /// <summary>
    ///     Create a patient
    /// </summary>
    /// <returns></returns>
    Task CreatePatient(CreatePatientDto dto);

    /// <summary>
    ///     Get user info
    /// </summary>
    /// <returns></returns>
    Task<BasicUserInfoDto> GetUserById(Guid userId);

    /// <summary>
    ///     Change user's info
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task ChangeBasicInfo(BasicInfoDto dto);

    /// <summary>
    ///     Change user's role
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="roleName"></param>
    /// <returns></returns>
    Task ChangeUserRole(Guid? userId, string roleName);

    /// <summary>
    ///     Change user's password
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task ChangePassword(ChangePasswordDto dto);


    /// <summary>
    ///     Admin reset user's password
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task ResetPassword(ResetPasswordDto dto);
}

