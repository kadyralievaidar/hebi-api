using Hebi_Api.Features.Clinics.Dtos;
using Hebi_Api.Features.Core.DataAccess.Models;
using Hebi_Api.Features.Core.DataAccess.UOW;
using Hebi_Api.Features.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace Hebi_Api.Features.Clinics.Services;

public class ClinicsService : IClinicsService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpContextAccessor _contextAccessor;

    public ClinicsService(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor)
    {
        _unitOfWork = unitOfWork;
        _contextAccessor = contextAccessor;
    }

    public async Task<Guid> CreateClinicAsync(CreateClinicDto dto)
    {
        var clinic = new Clinic()
        {
            Name = dto.Name,
            Location = dto.Location,
            PhoneNumber = dto.PhoneNumber,
            Email = dto.Email,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = _contextAccessor.GetUserIdentifier(),
            IsActive = true
        };
        await _unitOfWork.ClinicRepository.InsertAsync(clinic);

        var doctors = await _unitOfWork.UsersRepository
                        .WhereAsync(user => dto.DoctorIds.Contains(user.Id) && !user.IsDeleted);

        foreach (var doctor in doctors)
        {
            doctor.ClinicId = clinic.Id;
        }

        await _unitOfWork.UsersRepository.UpdateRangeAsync(doctors);
        await _unitOfWork.SaveAsync();
        return clinic.Id;
    }

    public async Task DeleteClinic(Guid id)
    {
        var clinic = await _unitOfWork.ClinicRepository.GetByIdAsync(id)
            ?? throw new NullReferenceException(nameof(Clinic));
        _unitOfWork.ClinicRepository.Delete(clinic);

        await _unitOfWork.SaveAsync();
    }

    public async Task<Clinic> GetClinicAsync(Guid clinicId)
    {
        var clinic = await _unitOfWork.ClinicRepository.GetByIdAsync(clinicId)
                ?? throw new NullReferenceException(nameof(Clinic));

        return clinic;
    }

    public async Task<List<Clinic>> GetListOfClinicsAsync(GetPagedListOfClinicDto dto)
    {
        var query = _unitOfWork.ClinicRepository.AsQueryable();
        var clinics = await query.OrderByDynamic(dto.SortBy, dto.SortDirection == ListSortDirection.Descending)
                .TrySkip(dto.PageSize * dto.PageIndex).TryTake(dto.PageSize).ToListAsync();

        return clinics;
    }

    public async Task<Clinic> UpdateClinicAsync(Guid id, CreateClinicDto dto)
    {
        var clinic = await _unitOfWork.ClinicRepository.GetByIdAsync(id) ?? 
            throw new NullReferenceException(nameof(Clinic));
        clinic.Location = dto.Location;
        clinic.Email = dto.Email;
        clinic.PhoneNumber = dto.PhoneNumber;
        clinic.Name = dto.Name;
        clinic.LastModifiedAt = DateTime.UtcNow;
        clinic.LastModifiedBy = _contextAccessor.GetUserIdentifier();

        _unitOfWork.ClinicRepository.Update(clinic);

        var doctors = await _unitOfWork.UsersRepository
                        .WhereAsync(user => dto.DoctorIds.Contains(user.Id) && !user.IsDeleted);

        foreach (var doctor in doctors)
        {
            doctor.ClinicId = clinic.Id;
        }

        await _unitOfWork.UsersRepository.UpdateRangeAsync(doctors);
        await _unitOfWork.SaveAsync();

        return clinic;
    }
}
