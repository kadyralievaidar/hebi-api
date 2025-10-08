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

    /// <summary>
    ///     Delete appointments
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task DeleteAppointment(Guid id);

    /// <summary>
    ///     Update appointments
    /// </summary>
    /// <param name="id"></param>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task<Appointment> UpdateAppointmentAsync(Guid id, UpdateAppointmentDto dto);

    /// <summary>
    ///     Get list of appointments based on filters
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task<List<AppointmentDto?>> GetListOfAppointmentsAsync(GetListOfAppointmentDto dto);

    /// <summary>
    ///     Get appointment by id
    /// </summary>
    /// <param name="appointmentId"></param>
    /// <returns></returns>
    Task<AppointmentDto?> GetAppointmentAsync(Guid appointmentId);


    /// <summary>
    ///     Generate appointments on shifts
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task GenerateAppointments(GenerateAppointmentsDto dto);
}
