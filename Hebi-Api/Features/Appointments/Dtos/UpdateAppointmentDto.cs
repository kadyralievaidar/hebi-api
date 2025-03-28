namespace Hebi_Api.Features.Appointments.Dtos;

public class UpdateAppointmentDto
{
    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }
    public Guid? DoctorId { get; set; }
    public Guid? ShiftId { get; set; }
}
