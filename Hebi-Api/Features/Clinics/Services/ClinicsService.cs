using Hebi_Api.Features.Clinics.Dtos;
using Hebi_Api.Features.Core.Common;
using Hebi_Api.Features.Core.Common.RequestHandling;
using Hebi_Api.Features.Core.DataAccess.Models;
using Hebi_Api.Features.Core.DataAccess.UOW;
using Hebi_Api.Features.Core.Extensions;
using Hebi_Api.Features.Users.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace Hebi_Api.Features.Clinics.Services;

public class ClinicsService : IClinicsService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly UserManager<ApplicationUser> _userManager;

    public ClinicsService(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor, UserManager<ApplicationUser> userManager)
    {
        _unitOfWork = unitOfWork;
        _contextAccessor = contextAccessor;
        _userManager = userManager;
    }

    public async Task<Guid> CreateClinicAsync(CreateClinicDto dto)
    {
        try
        {
            var userId = _contextAccessor.GetUserIdentifier();
            var user = await _unitOfWork.UsersRepository.FirstOrDefaultAsync(x => x.Id == userId);

            var clinic = new Clinic
            {
                Name = dto.Name,
                Location = dto.Location,
                PhoneNumber = dto.PhoneNumber,
                Email = dto.Email,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = user.Id,
                IsActive = true
            };
            await _unitOfWork.ClinicRepository.InsertAsync(clinic);

            var isSuperAdmin = await _userManager.IsInRoleAsync(user, Consts.SuperAdmin);

            if (!isSuperAdmin && !dto.DoctorIds.Contains(userId.Value))
                dto.DoctorIds.Add(userId.Value);

            var doctors = await _unitOfWork.UsersRepository
                .WhereAsync(x => dto.DoctorIds.Contains(x.Id) && !x.IsDeleted);

            if (doctors.Any())
            {
                foreach (var doctor in doctors)
                    doctor.ClinicId = clinic.Id;

                await _unitOfWork.UsersRepository.UpdateRangeAsync(doctors);
            }

            await _unitOfWork.SaveAsync();
            return clinic.Id;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }

    public async Task<Guid> CreateDefaultClinic()
    {
        return await _unitOfWork.ClinicRepository.CreateDefaultClinic();
    }

    public async Task DeleteClinic(Guid id)
    {
        try
        {
            var clinic = await _unitOfWork.ClinicRepository.GetClinicByDoctor(x => x.Id == id);
            foreach (var doctor in clinic.Doctors)
                doctor.ClinicId = null;

            await _unitOfWork.UsersRepository.UpdateRangeAsync(clinic.Doctors);

            _unitOfWork.ClinicRepository.Delete(clinic);

            await _unitOfWork.SaveAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }

    public async Task<ShortClinicInfo> GetClinicAsync(Guid clinicId)
    {
        var clinic = await _unitOfWork.ClinicRepository.GetClinicById(clinicId);
        var dto = new ShortClinicInfo()
        {
            Name = clinic.Name,
            Location = clinic.Location,
            Email = clinic.Email,
            PhoneNumber = clinic.PhoneNumber,
            Id = clinic.Id,
        };
        return dto;
    }

    public async Task<PagedResult<ShortClinicInfo>> GetListOfClinicsAsync(GetPagedListOfClinicDto dto)
    {
        var query = _unitOfWork.ClinicRepository.AsQueryable();
        if (!string.IsNullOrEmpty(dto.SearchText))
            query = query.Where(x => x.Name.Contains(dto.SearchText) || x.Location.Contains(dto.SearchText));

        var totalCount = await query.CountAsync();
        var clinics = await query.OrderByDynamic(dto.SortBy, dto.SortDirection == ListSortDirection.Descending)
                .TrySkip(dto.PageSize * dto.PageIndex).TryTake(dto.PageSize)
                .Select(x => new ShortClinicInfo()
                {
                    Name = x.Name,
                    Location = x.Location,
                    Email = x.Email,
                    PhoneNumber = x.PhoneNumber,
                    Id = x.Id,
                }).ToListAsync();

        return new PagedResult<ShortClinicInfo>()
        {
            Results = clinics,
            TotalCount = totalCount
        };
    }

    public async Task UpdateClinicAsync(Guid id, CreateClinicDto dto)
    {
        var clinic = await _unitOfWork.ClinicRepository.GetByIdAsync(id);
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
    }

    public async Task<ClinicWithDoctorsDto?> GetClinicWithDoctorsAsync(GetClinicsDoctorsDto dto)
    {
        var clinic = await _unitOfWork.ClinicRepository.GetClinicById(dto.ClinicId);

        var query = _unitOfWork.UsersRepository.AsQueryable().Where(x => x.ClinicId == dto.ClinicId && !x.IsDeleted);

        var count = await query.CountAsync();

        var allUsers = await query.TrySkip(dto.PageIndex * dto.PageSize).TryTake(dto.PageSize).ToListAsync();

        var doctors = new List<ApplicationUser>();

        foreach (var user in allUsers)
            if (await _userManager.IsInRoleAsync(user, Consts.Doctor))
                doctors.Add(user);

        return new ClinicWithDoctorsDto
        {
            ClinicId = clinic!.Id,
            ClinicName = clinic.Name!,
            Doctors = new PagedResult<BasicInfoDto>()
            {
                Results = doctors.Select(d => new BasicInfoDto
                {
                    UserId = d.Id,
                    Email = d.Email,
                    FirstName = d.FirstName,
                    LastName = d.LastName,
                    PhoneNumber = d.PhoneNumber,
                }).ToList(),
                TotalCount = count
            }
        };
    }

    public async Task RemoveDoctorsFromClinic(List<Guid> doctorIds)
    {
        var doctors = await _unitOfWork.UsersRepository.WhereAsync(x => doctorIds.Contains(x.Id));
        foreach (var doctor in doctors)
        {
            doctor.ClinicId = null;
            doctor.IsDeleted = true;
        }
        await _unitOfWork.UsersRepository.UpdateRangeAsync(doctors);
        await _unitOfWork.SaveAsync();
    }
}
