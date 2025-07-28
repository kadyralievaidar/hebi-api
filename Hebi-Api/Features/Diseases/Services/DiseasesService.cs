using Hebi_Api.Features.Core.DataAccess.Models;
using Hebi_Api.Features.Core.DataAccess.UOW;
using Hebi_Api.Features.Core.Extensions;
using Hebi_Api.Features.Diseases.Dtos;

namespace Hebi_Api.Features.Diseases.Services;

public class DiseasesService : IDiseaseService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpContextAccessor _contextAccessor;

    public DiseasesService(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor)
    {
        _unitOfWork = unitOfWork;
        _contextAccessor = contextAccessor;
    }

    public async Task<Guid> CreateDisease(CreateDiseaseDto dto)
    {
        var disease = new Disease() 
        {
            Name = dto.Name,
            Description = dto.Description,
            CreatedAt = DateTime.Now
        };
        await _unitOfWork.DiseaseRepository.InsertAsync(disease);
        await _unitOfWork.SaveAsync();

        return disease.Id;
    }

    public async Task DeleteDisease(Guid id)
    {
        var disease = await _unitOfWork.DiseaseRepository.GetByIdAsync(id);
        _unitOfWork.DiseaseRepository.Delete(disease!);
        await _unitOfWork.SaveAsync();
    }

    public async Task<Disease> GetDiseaseAsync(Guid id)
    {
        var disease = await _unitOfWork.DiseaseRepository.GetByIdAsync(id)
            ?? throw new NullReferenceException(nameof(Disease));

        return disease;
    }

    public async Task<List<Disease>> GetListOfDiseasesAsync(GetPagedListOfDiseaseDto dto)
    {
        var diseases = await _unitOfWork.DiseaseRepository.SearchAsync(x => dto.SearchText == null
        || (x.Description!.ToLower().Contains(dto.SearchText.ToLower()) || x.Name!.ToLower().Contains(dto.SearchText.ToLower())),
            dto.SortBy, dto.SortDirection, dto.PageSize * dto.PageIndex, dto.PageSize);

        return diseases;
    }

    public async Task<Disease> UpdateDisease(Guid id, CreateDiseaseDto dto)
    {
        var disease = await _unitOfWork.DiseaseRepository.GetByIdAsync(id)
                        ?? throw new NullReferenceException(nameof(Disease));

        disease.Name = dto.Name;
        disease.Description = dto.Description;
        disease.LastModifiedAt = DateTime.UtcNow;
        disease.LastModifiedBy = _contextAccessor.GetUserIdentifier();
        _unitOfWork.DiseaseRepository.Update(disease);
        await _unitOfWork.SaveAsync();
        return disease;
    }
}
