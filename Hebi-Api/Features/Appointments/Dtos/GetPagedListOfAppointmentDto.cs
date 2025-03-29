using Hebi_Api.Features.Core.Common.RequestHandling;

namespace Hebi_Api.Features.Appointments.Dtos;

public class GetPagedListOfAppointmentDto : BaseListRequest
{
    /// <summary>
    ///     Start date time
    /// </summary>
    public DateTime? StartDate { get; set; }

    /// <summary>
    ///     End date time
    /// </summary>
    public DateTime? EndDate { get; set; }

    /// <summary>
    ///     Patient's id
    /// </summary>
    public Guid? PatientId { get; set; }

    /// <summary>
    ///     Shift's id
    /// </summary>
    public Guid? ShiftId { get; set; }
}
