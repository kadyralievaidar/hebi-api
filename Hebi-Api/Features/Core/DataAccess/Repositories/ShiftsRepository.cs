using Hebi_Api.Features.Core.DataAccess.Interfaces;
using Hebi_Api.Features.Core.DataAccess.Models;

namespace Hebi_Api.Features.Core.DataAccess.Repositories;

public class ShiftsRepository : GenericRepository<Shift>, IShiftsRepository
{
    public ShiftsRepository(HebiDbContext context, IHttpContextAccessor contextAccessor) : base(context, contextAccessor)
    {
    }
}
