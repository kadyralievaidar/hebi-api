namespace Hebi_Api.Features.Clinics.Dtos;

/// <summary>
///     Create clinic dto
/// </summary>
public class CreateClinicDto
{
    /// <summary>
    ///     Name of clinic
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    ///     Address of clinic
    /// </summary>
    public string? Location { get; set; }

    /// <summary>
    ///     Phone number
    /// </summary>
    public string? PhoneNumber { get; set; }

    /// <summary>
    ///     Clinic's email
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    ///     Doctor ids
    /// </summary>
    public List<Guid> DoctorIds { get; set; } = new();
}
