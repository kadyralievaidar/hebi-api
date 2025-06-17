using System.ComponentModel;
using Hebi_Api.Features.Appointments.Dtos;
using Hebi_Api.Features.Core.DataAccess.Models;
using Hebi_Api.Features.Core.DataAccess.UOW;
using Hebi_Api.Features.Core.Extensions;
using Microsoft.EntityFrameworkCore;

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
    public async Task<Guid> CreateAppointmentAsync(CreateAppointmentDto dto)
    {
        var appointment = new Appointment()
        {
            Id = Guid.NewGuid(),
            StartDate = dto.StartDateTime,
            EndDate = dto.EndDateTime,
            DoctorId = dto.DoctorId ?? _contextAccessor.GetUserIdentifier(),
            PatientId = dto.PatientId,
            PatientShortName = dto.ShortName,
            FilePath = dto.FilePath,
            ShiftId = dto.ShiftId,
            CreatedAt = DateTime.UtcNow,
            IsDeleted = false,
            Name = dto.Name,
            Description = dto.Description,
            CreatedBy = _contextAccessor.GetUserIdentifier(),
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

    public async Task<Appointment> UpdateAppointmentAsync(Guid id, UpdateAppointmentDto dto) 
    {
        var appointment = await _unitOfWork.AppointmentRepository.GetByIdAsync(id)
                            ?? throw new NullReferenceException(nameof(Appointment));

        appointment.PatientId = dto.PatientId ?? Guid.Empty;
        appointment.DoctorId = dto.DoctorId ?? Guid.Empty;
        appointment.StartDate = dto.StartDateTime;
        appointment.EndDate = dto.EndDateTime;
        appointment.Description = dto.Description;
        appointment.Name = dto.Name;
        appointment.ShiftId = dto.ShiftId ?? Guid.Empty;
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
        var query = _unitOfWork.AppointmentRepository.AsQueryable().AsNoTracking();

        if (dto.ShiftId != null)
        {
            query = query.Where(x => x.ShiftId == dto.ShiftId);
        }

        if (dto.PatientId != null)
        {
            query = query.Where(x => x.PatientId == dto.PatientId);
        }

        if (dto.StartDate.HasValue && dto.EndDate.HasValue)
        {
            query = query.Where(x => x.StartDate >= dto.StartDate.Value && x.EndDate <= dto.EndDate.Value);
        }
        query = query.OrderByDynamic(dto.SortBy, dto.SortDirection == ListSortDirection.Ascending);
        query = query.Skip(dto.PageIndex * dto.PageSize).Take(dto.PageSize);

        // Execute
        var appointments = await query.ToListAsync();
        return appointments;
    }
}
