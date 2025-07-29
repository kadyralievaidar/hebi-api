using Hebi_Api.Features.Appointments.Dtos;
using Hebi_Api.Features.Users.Dtos;

namespace Hebi_Api.Features.UserCards.Dtos;

public class UserCardResponseDto
{
    /// <summary>
    ///     Client's basic info
    /// </summary>
    public BasicInfoDto UserInfo { get; set; } = null!;

    /// <summary>
    ///     Appointments
    /// </summary>
    public List<AppointmentDto> Appointments { get; set; } = [];
}
