using Hebi_Api.Features.Core.DataAccess.Interfaces;
using Hebi_Api.Features.Core.DataAccess.Models;

namespace Hebi_Api.Features.Core.DataAccess.Repositories;

public class ClinicsRepository : GenericRepository<Clinic>, IClinicsRepository
{
    public ClinicsRepository(HebiDbContext context, IHttpContextAccessor contextAccessor) : base(context, contextAccessor)
    {
    }
}
