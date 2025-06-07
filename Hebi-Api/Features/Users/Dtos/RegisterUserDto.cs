using System.ComponentModel.DataAnnotations;

namespace Hebi_Api.Features.Users.Dtos;

/// <summary>
///     User registration dto
/// </summary>
public class RegisterUserDto
{
    /// <summary>
    ///     User's username
    /// </summary>
    public required string UserName { get; set; }

    /// <summary>
    ///     User's password
    /// </summary>
    [DataType(DataType.Password)]
    public required string Password { get; set; }

    /// <summary>
    ///     Password's confirmation
    /// </summary>
    [Compare("Password", ErrorMessage = "Passwords don't match.")]
    public required string ConfirmPassword { get; set; }

    /// <summary>
    ///     User's email
    /// </summary>
    [EmailAddress]
    public required string Email { get; set; }

    /// <summary>
    ///     User's firstname
    /// </summary>
    public required string FirstName { get; set; }

    /// <summary>
    ///     User's lastname
    /// </summary>
    public required string LastName { get; set; }

    /// <summary>
    ///     User's phone number
    /// </summary>
    public required string PhoneNumber { get; set; }
}
