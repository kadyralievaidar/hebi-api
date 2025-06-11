namespace Hebi_Api.Features.Users.Dtos;

/// <summary>
///     Create user for clinic
/// </summary>
public class CreateUserDto
{
    /// <summary>
    ///     Register user dto
    /// </summary>
    public RegisterUserDto RegisterDto { get; set; }

    /// <summary>
    ///     Clinic's Id
    /// </summary>
    public Guid ClinicId { get; set; }

    /// <summary>
    ///  ctr
    /// </summary>
    /// <param name="registerDto"></param>
    /// <param name="clinicId"></param>
    public CreateUserDto(RegisterUserDto registerDto, Guid clinicId)
    {
        RegisterDto = registerDto;
        ClinicId = clinicId;
    }
}
