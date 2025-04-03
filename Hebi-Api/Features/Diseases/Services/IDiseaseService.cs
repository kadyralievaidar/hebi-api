using Hebi_Api.Features.Core.DataAccess.Models;
using Hebi_Api.Features.Diseases.Dtos;

namespace Hebi_Api.Features.Diseases.Services;

public interface IDiseaseService
{
    /// <summary>
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task<Guid> CreateDisease(CreateDiseaseDto dto);

    Task DeleteDisease(Guid id);

    Task<Disease> UpdateDisease(Guid id, CreateDiseaseDto dto);

    Task<List<Disease>> GetListOfDiseasesAsync(GetPagedListOfDiseaseDto dto);

    Task<Disease> GetDiseaseAsync(Guid id);
}
