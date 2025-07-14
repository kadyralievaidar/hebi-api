using Hebi_Api.Features.Core.DataAccess.Models;

namespace Hebi_Api.Features.Core.DataAccess.Interfaces;

public interface IUsersRepository : IGenericRepository<ApplicationUser>
{
    Task<ApplicationUser> GetUsersWithClinic(Guid userId);
}
