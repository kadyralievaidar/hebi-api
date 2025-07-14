using Hebi_Api.Features.Clinics.Dtos;
using Hebi_Api.Features.Core.Common.RequestHandling;
using Hebi_Api.Features.Core.DataAccess.Models;

namespace Hebi_Api.Features.Clinics.Services;

public interface IClinicsService 
{
    /// <summary>
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task<Guid> CreateClinicAsync(CreateClinicDto dto);

    /// <summary>
    ///     Delete clinic by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task DeleteClinic(Guid id);

    /// <summary>
    ///     Update clinic 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task UpdateClinicAsync(Guid id, CreateClinicDto dto);

    /// <summary>
    ///     Get list of clinics
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task<PagedResult<ShortClinicInfo>> GetListOfClinicsAsync(GetPagedListOfClinicDto dto);

    /// <summary>
    ///     Get clinic by id
    /// </summary>
    /// <param name="clinicId"></param>
    /// <returns></returns>
    Task<ShortClinicInfo> GetClinicAsync(Guid clinicId);

    /// <summary>
    ///     Create deafult clinic in case of individual
    /// </summary>
    /// <returns></returns>
    Task<Guid> CreateDefaultClinic();

    /// <summary>
    ///     Get clinic with doctors
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task<ClinicWithDoctorsDto?> GetClinicWithDoctorsAsync(GetClinicsDoctorsDto dto);

    /// <summary>
    ///     Remove doctors from clinic
    /// </summary>
    /// <param name="doctorIds"></param>
    /// <returns></returns>
    Task RemoveDoctorsFromClinic(List<Guid> doctorIds);
}
