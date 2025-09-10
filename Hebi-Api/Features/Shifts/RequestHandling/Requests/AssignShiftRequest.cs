using Hebi_Api.Features.Core.Common.RequestHandling;

namespace Hebi_Api.Features.Shifts.RequestHandling.Requests;

/// <summary>
///     Shift's assign request
/// </summary>
public class AssignShiftRequest : Request<Response>
{
    /// <summary>
    ///     Doctor's id (can be null)
    /// </summary>
    public Guid? DoctorId { get; set; }

    /// <summary>
    ///     Shift's id can not be null
    /// </summary>
    public Guid ShiftId { get; set; }
    public AssignShiftRequest(Guid? doctorId, Guid shiftId)
    {
        DoctorId = doctorId;
        ShiftId = shiftId;
    }
}
