namespace Hebi_Api.Features.Users.Dtos;

/// <summary>
///     Basic user's info
/// </summary>
public class BasicUserInfoDto : BasicInfoDto
{
    /// <summary>
    ///     Clinic's name
    /// </summary>
    public string? ClinicName { get; set; }
}
