namespace Hebi_Api.Features.Users.Dtos;

/// <summary>
///     Reset user's password
/// </summary>
public class ResetPasswordDto
{
    /// <summary>
    ///     User's id
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    ///     User's new password
    /// </summary>
    public string Password { get; set; } = null!;
}
