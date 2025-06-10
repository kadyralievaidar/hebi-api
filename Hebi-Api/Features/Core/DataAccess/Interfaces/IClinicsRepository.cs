using Hebi_Api.Features.Core.DataAccess.Models;

namespace Hebi_Api.Features.Core.DataAccess.Interfaces;

public interface IClinicsRepository : IGenericRepository<Clinic>
{
    /// <summary>
    ///     Returns clinic
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Clinic> GetClinicById(Guid id);
}
