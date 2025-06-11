using Hebi_Api.Features.Core.DataAccess.Interfaces;
using Hebi_Api.Features.Core.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace Hebi_Api.Features.Core.DataAccess.Repositories;

public class ClinicsRepository : GenericRepository<Clinic>, IClinicsRepository
{
    private readonly HebiDbContext _dbContext;
    public ClinicsRepository(HebiDbContext context, IHttpContextAccessor contextAccessor) : base(context, contextAccessor)
    {
        _dbContext = context;
    }

    /// <inheritdoc/>
    public async Task<Clinic?> GetClinicById(Guid id)
    {
        return await _dbContext.Clinics.FirstOrDefaultAsync(x => x.Id == id);
    }
}
