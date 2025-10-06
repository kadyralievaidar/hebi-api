using System.ComponentModel;
using Hebi_Api.Features.Appointments.Dtos;
using Hebi_Api.Features.Core.Common.RequestHandling;
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
        var userCard = await _unitOfWork.UserCardsRepository.GetByIdAsync(dto.UserCardId!.Value);
        var appointment = new Appointment()
        {
            Id = Guid.NewGuid(),
            StartDate = dto.StartDateTime,
            EndDate = dto.EndDateTime,
            DoctorId = dto.DoctorId ?? _contextAccessor.GetUserIdentifier(),
            PatientId = userCard?.PatientId ?? null,
            PatientShortName = dto.ShortName,
            FilePath = dto.FilePath,
            ShiftId = dto.ShiftId,
            CreatedAt = DateTime.UtcNow,
            IsDeleted = false,
            Name = dto.Name,
            DiseaseId = dto.DiseaseId,
            Description = dto.Description,
            CreatedBy = _contextAccessor.GetUserIdentifier(),
            UserCardId = dto.UserCardId
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

    public async Task<AppointmentDto?> GetAppointmentAsync(Guid appointmentId) 
    {
        var appointment = await _unitOfWork.AppointmentRepository
            .SearchQuery(
                a => a.Id == appointmentId,
                relations: new List<string>
                {
                nameof(Appointment.UserCard),
                nameof(Appointment.Disease),
                nameof(Appointment.Patient),
                nameof(Appointment.Doctor)
                })
            .Select(appointment => new AppointmentDto(appointment))
            .FirstOrDefaultAsync();

        return appointment;
    }

    public async Task<PagedResult<AppointmentDto?>> GetListOfAppointmentsAsync(GetPagedListOfAppointmentDto dto)
    {
        var query = _unitOfWork.AppointmentRepository.AsQueryable()
                                                    .Include(x => x.Disease)
                                                    .Include(x => x.UserCard)
                                                    .Include(x => x.Doctor)
                                                    .Include(x => x.Patient)
                                                    .AsNoTracking();

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
        var totalCount = await query.CountAsync();
        query = query.Skip(dto.PageIndex * dto.PageSize).Take(dto.PageSize);

        // Execute
        var appointments = await query.Select(appointment => new AppointmentDto(appointment)).ToListAsync();

        return new PagedResult<AppointmentDto?>()
        {
            Results = appointments,
            TotalCount = totalCount
        };
    }

    public async Task GenerateAppointments(GenerateAppointmentsDto dto)
    {
        var userCard = await _unitOfWork.UserCardsRepository.GetByIdAsync(dto.UserCardId.Value);

        foreach (var shiftId in dto.ShiftIds)
        {
            var shift = await _unitOfWork.ShiftsRepository.FirstOrDefaultAsync(s => s.Id == shiftId);
            if (shift == null) continue;

            var newStart = shift.StartTime.AddTicks(dto.StartTime.Ticks);
            var newEnd = shift.StartTime.AddTicks(dto.EndTime.Ticks);

            var hasConflict = await _unitOfWork.AppointmentRepository
                .AnyAsync(a => a.ShiftId == shiftId &&
                              ((newStart >= a.StartDate && newStart < a.EndDate) ||
                               (newEnd > a.StartDate && newEnd <= a.EndDate) ||
                               (newStart <= a.StartDate && newEnd >= a.EndDate)));

            if (hasConflict)
            {
                continue;
            }

            var appointment = new Appointment
            {
                StartDate = newStart,
                EndDate = newEnd,
                UserCardId = dto.UserCardId,
                PatientId = userCard.PatientId,
                DiseaseId = dto.DiseaseId,
                ShiftId = shift.Id
            };

            await _unitOfWork.AppointmentRepository.InsertAsync(appointment);
        }

        await _unitOfWork.SaveAsync();
    }
}
