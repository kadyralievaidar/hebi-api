namespace Hebi_Api.Features.Appointments.Dtos;

public class CreateAppointmentDto
{
    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }
    public Guid? DoctorId { get; set; }
    public Guid? PatientId { get; set; }
    public string? ShortName { get; set; }
}
