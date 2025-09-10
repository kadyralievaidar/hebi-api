namespace Hebi_Api.Features.Users.Dtos;

/// <summary>
///     User's change password dto
/// </summary>
public class ChangePasswordDto
{
    /// <summary>
    ///     User's old password
    /// </summary>
    public string OldPassword { get; set; } = null!;

    /// <summary>
    ///     User's new password
    /// </summary>
    public string Password { get; set; } = null!;
}
