using System.Linq.Expressions;
using Hebi_Api.Features.Core.DataAccess.Interfaces;
using Hebi_Api.Features.Core.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace Hebi_Api.Features.Core.DataAccess.Repositories;

public class UsersRepository : GenericRepository<ApplicationUser>, IUsersRepository
{
    private readonly HebiDbContext _context;
    public UsersRepository(HebiDbContext context, IHttpContextAccessor contextAccessor) : base(context, contextAccessor)
    {
        _context = context;
    }

    /// <summary>
    ///     Return an entity. Similar to LINQ FirstOrDefault
    /// </summary>
    /// <param name="filter">Filter for selection condition</param>
    /// <returns>Entity</returns>
    public override async Task<ApplicationUser?> FirstOrDefaultAsync(Expression<Func<ApplicationUser, bool>>? filter = null)
    {
        var test = await _context.Users.ToListAsync();
        return await _context.Users.Include(x => x.Clinic).FirstOrDefaultAsync(filter!);
    }

    /// <summary>
    ///     Return an entity. Similar to LINQ FirstOrDefault
    /// </summary>
    /// <param name="filter">Filter for selection condition</param>
    /// <returns>Entity</returns>
    public override async Task<bool> AnyAsync(Expression<Func<ApplicationUser, bool>> predicate)
    {
        return await _context.Users.AnyAsync(predicate);
    }

    /// <summary>
    ///     Return filtered users
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public override async Task<IEnumerable<ApplicationUser>> WhereAsync(Expression<Func<ApplicationUser, bool>>? filter = null)
    {
        return await _context.Users.Where(filter).ToListAsync();
    }
}
