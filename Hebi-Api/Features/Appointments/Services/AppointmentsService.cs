using Hebi_Api.Features.Appointments.Dtos;
using Hebi_Api.Features.Core.DataAccess.Models;
using Hebi_Api.Features.Core.DataAccess.UOW;
using Hebi_Api.Features.Core.Extensions;

namespace Hebi_Api.Features.Appointments.Services;

public class AppointmentsService : IAppointmentsService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpContextAccessor _contextAccessor;

    public AppointmentsService(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor)
    {
        _unitOfWork = unitOfWork;
        _contextAccessor = contextAccessor;
    }
    public async Task<Guid> CreateAppointment(CreateAppointmentDto dto)
    {
        var appointment = new Appointment()
        {
            Id = Guid.NewGuid(),
            StartDate = dto.StartDateTime,
            EndDate = dto.EndDateTime,
            DoctorId = dto.DoctorId ?? Guid.Empty,
            PatientId = dto.PatientId ?? Guid.Empty,
            PatientShortName = dto.ShortName,
            FilePath = dto.FilePath,
            ShiftId = dto.ShiftId ?? Guid.Empty,
            CreatedAt = DateTime.UtcNow,
            IsDeleted = false,
            Name = dto.Name,
            Description = dto.Description,
            CreatedBy = _contextAccessor.GetUserIdentifier()
        };
        await _unitOfWork.AppointmentRepository.InsertAsync(appointment);
        await _unitOfWork.SaveAsync();
        return appointment.Id;
    }

    public async Task DeleteAppointment(Guid id)
    {
        var appointment = await _unitOfWork.AppointmentRepository.GetByIdAsync(id);

        _unitOfWork.AppointmentRepository.Delete(appointment!);
        await _unitOfWork.SaveAsync();
    }

    public async Task<Appointment> UpdateAppointment(Guid id, UpdateAppointmentDto dto) 
    {
        var appointment = await _unitOfWork.AppointmentRepository.GetByIdAsync(id)
                            ?? throw new NullReferenceException(nameof(Appointment));

        appointment.ShiftId = dto.ShiftId.Value;
        appointment.LastModifiedBy = _contextAccessor.GetUserIdentifier();
        appointment.LastModifiedAt = DateTime.UtcNow;
        _unitOfWork.AppointmentRepository.Update(appointment);
        await _unitOfWork.SaveAsync();
        return appointment;
    }

    public async Task<Appointment> GetAppointmentAsync(Guid appointmentId) 
    {
        var appointment = await _unitOfWork.AppointmentRepository.GetByIdAsync(appointmentId);
        return appointment!;
    }

    public async Task<List<Appointment>> GetListOfAppointmentsAsync(GetPagedListOfAppointmentDto dto)
    {
        var appointments = await _unitOfWork.AppointmentRepository.SearchAsync(x => x.ShiftId == dto.ShiftId 
                                                                    && (x.StartDate >= dto.StartDate && x.EndDate <= dto.EndDate)
                                                                    && x.PatientId == dto.PatientId, dto.SortBy, dto.SortDirection,
                                                                    (dto.PageIndex * dto.PageSize), dto.PageSize);
        return appointments;
    }
}
