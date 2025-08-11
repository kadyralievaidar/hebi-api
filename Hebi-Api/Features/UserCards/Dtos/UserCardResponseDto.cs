using Hebi_Api.Features.Appointments.Dtos;
using Hebi_Api.Features.Users.Dtos;

namespace Hebi_Api.Features.UserCards.Dtos;

/// <summary>
///     User card response dto
/// </summary>
public class UserCardResponseDto
{
    /// <summary>
    ///     User card's Id
    /// </summary>
    public Guid UserCardId { get; set; }
    /// <summary>
    ///     Client's basic info
    /// </summary>
    public BasicInfoDto UserInfo { get; set; } = null!;

    /// <summary>
    ///     Appointments
    /// </summary>
    public List<AppointmentDto> Appointments { get; set; } = [];
}
