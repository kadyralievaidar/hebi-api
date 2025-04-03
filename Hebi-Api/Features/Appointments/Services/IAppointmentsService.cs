using Hebi_Api.Features.Appointments.Dtos;
using Hebi_Api.Features.Core.DataAccess.Models;

namespace Hebi_Api.Features.Appointments.Services;

public interface IAppointmentsService
{
    /// <summary>
    ///     Create an appointment
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task<Guid> CreateAppointment(CreateAppointmentDto dto);

    Task DeleteAppointment(Guid id);

    Task<Appointment> UpdateAppointment(Guid id, UpdateAppointmentDto dto);

    Task<List<Appointment>> GetListOfAppointmentsAsync(GetPagedListOfAppointmentDto dto);

    Task<Appointment> GetAppointmentAsync(Guid appointmentId);
}
