using Hebi_Api.Features.Core.DataAccess.Models;
using System.Linq.Expressions;

namespace Hebi_Api.Features.Core.DataAccess.Interfaces;

public interface IClinicsRepository : IGenericRepository<Clinic>
{
    /// <summary>
    ///     Returns clinic
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Clinic> GetClinicById(Guid id);

    /// <summary>
    ///     Get clinic if doctor is in it
    /// </summary>
    /// <param name="pred"></param>
    /// <returns></returns>
    Task<Clinic> GetClinicByDoctor(Expression<Func<Clinic, bool>>? filter = null);
}
