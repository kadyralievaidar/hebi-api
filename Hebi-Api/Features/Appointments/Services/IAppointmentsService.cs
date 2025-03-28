using Hebi_Api.Features.Appointments.Dtos;

namespace Hebi_Api.Features.Appointments.Services;

public interface IAppointmentsService
{
    /// <summary>
    ///     Create an appointment
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task<Guid> CreateAppointment(CreateAppointmentDto dto); 
}
