using Hebi_Api.Features.Core.Common.RequestHandling;

namespace Hebi_Api.Features.Shifts.Dtos;

public class GetPagedListOfShiftsDto : BaseListRequest
{
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public Guid DoctorId { get; set; }
}
