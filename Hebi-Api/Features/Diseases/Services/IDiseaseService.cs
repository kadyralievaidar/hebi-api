using Hebi_Api.Features.Core.Common.RequestHandling;
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

    /// <summary>
    ///     Delete disease by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task DeleteDisease(Guid id);

    /// <summary>
    ///     Update disease
    /// </summary>
    /// <param name="id"></param>
    /// <param name="dto"></param>
    /// <returns></returns>

    Task<Disease> UpdateDisease(Guid id, CreateDiseaseDto dto);

    /// <summary>
    ///     Get list of diseases
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task<PagedResult<Disease>> GetListOfDiseasesAsync(GetPagedListOfDiseaseDto dto);

    /// <summary>
    ///     Get disease by Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Disease> GetDiseaseAsync(Guid id);
}
