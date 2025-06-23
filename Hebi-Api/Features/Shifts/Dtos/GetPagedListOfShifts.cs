using Hebi_Api.Features.Core.Common.RequestHandling;

namespace Hebi_Api.Features.Shifts.Dtos;

public class GetPagedListOfShiftsDto : BaseListRequest
{
    public DateTime StartTime { get; set; } = DateTime.MinValue;
    public DateTime EndTime { get; set; } = DateTime.MaxValue;
    public Guid DoctorId { get; set; }
}
