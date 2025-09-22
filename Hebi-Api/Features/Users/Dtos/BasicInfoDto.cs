using Hebi_Api.Features.Core.Common.Enums;
using Hebi_Api.Features.Core.DataAccess.Models;

namespace Hebi_Api.Features.Users.Dtos;

/// <summary>
///     Basic user's info
/// </summary>
public class BasicInfoDto
{
    /// <summary>
    ///     User's Id
    /// </summary>
    public Guid UserId { get; set; }
    /// <summary>
    ///     User name
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    ///     User's first name
    /// </summary>
    public string? FirstName { get; set; }

    /// <summary>
    ///     User's last name
    /// </summary>
    public string? LastName { get; set; }

    /// <summary>
    ///     User's last name
    /// </summary>
    public string? PhoneNumber { get; set; }

    /// <summary>
    ///     User's sex
    /// </summary>
    public Sex Sex { get; set; }

    public BasicInfoDto() { }
    public BasicInfoDto(ApplicationUser? user)
    {
        if(user != null)
        {
            UserId = user.Id;
            Email = user.Email;
            FirstName = user.FirstName;
            LastName = user.LastName;
            PhoneNumber = user.PhoneNumber;
            Sex = user.Sex;
        }
    }
}
