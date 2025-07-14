using Hebi_Api.Features.Core.DataAccess.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Hebi_Api.Features.Core.DataAccess;

public class HebiDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="HebiDbContext"/> class.
    /// </summary>
    /// <param name="options">Database context options</param>
    public HebiDbContext(DbContextOptions<HebiDbContext> options) : base(options) { }

    /// <summary>
    ///     Initializes a new instance of the <see cref="HebiDbContext"/> class.
    /// </summary>
    public HebiDbContext() { }

    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<Clinic> Clinics { get; set; }
    public DbSet<Disease> Diseases { get; set; }
    public DbSet<Shift> Shifts { get; set; }
    public DbSet<UserCard> PatientCard { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(HebiDbContext).Assembly);
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(IBaseModel).IsAssignableFrom(entityType.ClrType))
            {
                var parameter = Expression.Parameter(entityType.ClrType, "e");
                var isDeletedProperty = Expression.Property(parameter, nameof(IBaseModel.IsDeleted));
                var isNotDeleted = Expression.Equal(isDeletedProperty, Expression.Constant(false));
                var lambda = Expression.Lambda(isNotDeleted, parameter);

                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
            }
        }
        base.OnModelCreating(modelBuilder);
    }
}
