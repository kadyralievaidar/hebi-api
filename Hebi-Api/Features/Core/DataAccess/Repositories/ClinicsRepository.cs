using Hebi_Api.Features.Core.DataAccess.Interfaces;
using Hebi_Api.Features.Core.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Hebi_Api.Features.Core.DataAccess.Repositories;

public class ClinicsRepository : GenericRepository<Clinic>, IClinicsRepository
{
    private readonly HebiDbContext _dbContext;
    public ClinicsRepository(HebiDbContext context, IHttpContextAccessor contextAccessor) : base(context, contextAccessor)
    {
        _dbContext = context;
    }

    public async Task<Clinic?> GetClinicByDoctor(Expression<Func<Clinic, bool>>? filter = null)
    {
        var test = await _dbContext.Clinics.ToListAsync();
        return await _dbContext.Clinics.Include(x => x.Doctors).FirstOrDefaultAsync(filter);
    }

    /// <inheritdoc/>
    public async Task<Clinic?> GetClinicById(Guid id)
    {
        return await _dbContext.Clinics.FirstOrDefaultAsync(x => x.Id == id);
    }
}
