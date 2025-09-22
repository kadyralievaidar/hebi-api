using Hebi_Api.Features.Core.DataAccess.Models;
using Hebi_Api.Features.Users.Dtos;

namespace Hebi_Api.Features.Shifts.Dtos;

/// <summary>
///     Shift's info
/// </summary>
public class ShiftDto
{
    /// <summary>
    ///     Shift's id
    /// </summary>
    public Guid ShiftId { get; set; }
    /// <summary>
    ///     Shift's start time
    /// </summary>
    public DateTime StartTime { get; set; }

    /// <summary>
    ///     Shift's end time
    /// </summary>
    public DateTime EndTime { get; set; }

    /// <summary>
    ///     Appointments collection
    /// </summary>
    public IEnumerable<Appointment> Appointments { get; set; } = new List<Appointment>();

    /// <summary>
    ///     Basic info doctor
    /// </summary>
    public BasicInfoDto DoctorInfo { get; set; }

    public ShiftDto() { }
    public ShiftDto(Shift? shift)
    {
        if(shift != null)
        {
            ShiftId = shift.Id;
            StartTime = shift.StartTime;
            EndTime = shift.EndTime;
            Appointments = shift.Appointments;
            DoctorInfo = new BasicInfoDto(shift.Doctor);
        }
    }
}
