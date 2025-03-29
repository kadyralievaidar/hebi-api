using Hebi_Api.Features.Core.DataAccess.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Hebi_Api.Features.Core.DataAccess;

public class HebiDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
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
        base.OnModelCreating(modelBuilder);
    }
}
