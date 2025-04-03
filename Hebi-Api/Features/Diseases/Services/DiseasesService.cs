using AutoMapper;
using Hebi_Api.Features.Core.DataAccess.Models;
using Hebi_Api.Features.Core.DataAccess.UOW;
using Hebi_Api.Features.Diseases.Dtos;

namespace Hebi_Api.Features.Diseases.Services;

public class DiseasesService : IDiseaseService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _contextAccessor;

    public DiseasesService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor contextAccessor)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _contextAccessor = contextAccessor;
    }

    public async Task<Guid> CreateDisease(CreateDiseaseDto dto)
    {
        var disease = _mapper.Map<Disease>(dto);
        await _unitOfWork.DiseaseRepository.InsertAsync(disease);
        await _unitOfWork.SaveAsync();

        return disease.Id;
    }

    public async Task DeleteDisease(Guid id)
    {
        var disease = await _unitOfWork.DiseaseRepository.GetByIdAsync(id)
                    ?? throw new NullReferenceException(nameof(Disease));
        _unitOfWork.DiseaseRepository.Delete(disease);
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
        var diseases = await _unitOfWork.DiseaseRepository.SearchAsync(x => dto.SearchText != null ? 
            x.Name.Contains(dto.SearchText) || x.Description.Contains(dto.SearchText) : true,
            dto.SortBy, dto.SortDirection, dto.PageSize * dto.PageIndex, dto.PageSize);

        return diseases;
    }

    public async Task<Disease> UpdateDisease(Guid id, CreateDiseaseDto dto)
    {
        var disease = await _unitOfWork.DiseaseRepository.GetByIdAsync(id)
                        ?? throw new NullReferenceException(nameof(Disease));

        disease = _mapper.Map<Disease>(dto);
        _unitOfWork.DiseaseRepository.Update(disease);
        await _unitOfWork.SaveAsync();
        return disease;
    }
}
