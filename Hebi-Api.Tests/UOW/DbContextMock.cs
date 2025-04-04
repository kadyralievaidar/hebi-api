using Hebi_Api.Features.Core.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Hebi_Api.Tests.UOW;
public class DbContextMock : HebiDbContext
{
    public DbContextMock(DbContextOptions<HebiDbContext> options) : base(options)
    {
    }
}

