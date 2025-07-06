namespace Hebi_Api.Features.Clinics.Dtos;

/// <summary>
///     Short clinic info
/// </summary>
public class ShortClinicInfo
{
    /// <summary>
    ///     CLinic's Id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    ///     Clinic's name
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    ///     Clinic's location
    /// </summary>
    public string? Location { get; set; }

    /// <summary>
    ///     Phone number
    /// </summary>
    public string? PhoneNumber { get; set; }

    /// <summary>
    ///     Email
    /// </summary>
    public string? Email { get; set; }
}
