using Hebi_Api.Features.Core.Common.Enums;

namespace Hebi_Api.Features.Users.Dtos;

/// <summary>
///     Patient's creation
/// </summary>
public class CreatePatientDto
{
    /// <summary>
    ///     User's firstname
    /// </summary>
    public required string FirstName { get; set; }

    /// <summary>
    ///     User's lastname
    /// </summary>
    public required string LastName { get; set; }

    /// <summary>
    ///     Patient's birthdate
    /// </summary>
    public DateTime? BirthDate { get; set; }

    /// <summary>
    ///     Patient's sex
    /// </summary>
    public Sex Sex { get; set; }

    /// <summary>
    ///     Patient's phone number
    /// </summary>
    public string? PhoneNumber { get; set; }
}
