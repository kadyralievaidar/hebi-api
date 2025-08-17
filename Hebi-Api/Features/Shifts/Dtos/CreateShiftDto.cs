namespace Hebi_Api.Features.Shifts.Dtos;

public class CreateShiftDto
{
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public Guid? DoctorId { get; set; } 
    public Guid? ShiftTemplateId { get; set; } 
    public IEnumerable<Guid> AppointmentIds { get; set; } = Enumerable.Empty<Guid>();
}
