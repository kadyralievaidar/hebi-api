using AutoMapper;
using Hebi_Api.Features.Clinics.Dtos;
using Hebi_Api.Features.Core.DataAccess.Models;
using Hebi_Api.Features.Core.DataAccess.UOW;

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

    public async Task<Guid> CreateClinic(CreateClinicDto dto)
    {
        var clininc = new Clinic()
        {
        };
        await _unitOfWork.ClinicRepository.InsertAsync(clininc);
        await _unitOfWork.SaveAsync();
        return clininc.Id;
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
        var clinics = await _unitOfWork.ClinicRepository.SearchAsync(x =>
                                        true, dto.SortBy, dto.SortDirection, 
                                        dto.PageSize * dto.PageIndex, dto.PageSize);
        return clinics;
    }

    public async Task<Clinic> UpdateClinic(Guid id, CreateClinicDto dto)
    {
        var clinic = await _unitOfWork.ClinicRepository.GetByIdAsync(id) ?? 
            throw new NullReferenceException(nameof(Clinic));

        _unitOfWork.ClinicRepository.Update(clinic);
        await _unitOfWork.SaveAsync();

        return clinic;
    }
}
