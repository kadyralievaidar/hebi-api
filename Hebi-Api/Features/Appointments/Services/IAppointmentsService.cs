using Hebi_Api.Features.Appointments.Dtos;
using Hebi_Api.Features.Core.Common.RequestHandling;
using Hebi_Api.Features.Core.DataAccess.Models;

namespace Hebi_Api.Features.Appointments.Services;

public interface IAppointmentsService
{
    /// <summary>
    ///     Create an appointment
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task<Guid> CreateAppointmentAsync(CreateAppointmentDto dto);

    Task DeleteAppointment(Guid id);

    Task<Appointment> UpdateAppointmentAsync(Guid id, UpdateAppointmentDto dto);

    Task<PagedResult<Appointment>> GetListOfAppointmentsAsync(GetPagedListOfAppointmentDto dto);

    Task<Appointment> GetAppointmentAsync(Guid appointmentId);
}
