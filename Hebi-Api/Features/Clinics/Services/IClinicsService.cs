using Hebi_Api.Features.Clinics.Dtos;
using Hebi_Api.Features.Core.DataAccess.Models;

namespace Hebi_Api.Features.Clinics.Services;

public interface IClinicsService 
{
    /// <summary>
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task<Guid> CreateClinic(CreateClinicDto dto);

    Task DeleteClinic(Guid id);

    Task<Clinic> UpdateClinic(Guid id, CreateClinicDto dto);

    Task<List<Clinic>> GetListOfClinicsAsync(GetPagedListOfClinicDto dto);

    Task<Clinic> GetClinicAsync(Guid clinicId);
}
