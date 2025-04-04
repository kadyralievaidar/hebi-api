using Hebi_Api.Features.Core.Common;
using Hebi_Api.Features.Core.DataAccess;
using Hebi_Api.Features.Core.DataAccess.Models;
using Hebi_Api.Features.Core.DataAccess.UOW;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Security.Claims;

namespace Hebi_Api.Tests.UOW;
internal class UnitOfWorkFactory : IDisposable
{
    public static Guid ClinicId = Guid.NewGuid();
    internal SqliteConnection _connection;
    internal DbContextMock _context;

    internal Mock<IHttpContextAccessor> _contextAccessor = new() { CallBase = true };

    internal UnitOfWorkFactory()
    {
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<HebiDbContext>()
            .UseSqlite(_connection)
            .EnableSensitiveDataLogging()
            .Options;


        _context = new DbContextMock(options);

        ClearData();
    }

    /// <inheritdoc />
    public void Dispose()
    {
        _connection?.Dispose();
        _context.Database.EnsureDeleted();
        _context?.Dispose();
    }

    internal void AddData<T>(List<T> data) where T : BaseModel
    {
        var type = typeof(DbContextMock);
        var prop = type.GetProperties().FirstOrDefault(x => x.PropertyType == typeof(DbSet<T>));
        var getter = prop.GetGetMethod();

        var dbSet = (DbSet<T>)getter.Invoke(_context, null);
        dbSet.AddRange(data);

        _context.SaveChanges();
    }

    private void ClearData()
    {
        if (_context.Database.EnsureCreated())
        {
            foreach (var entity in _context.Shifts)
            {
                _context.Shifts.Remove(entity);
                _context.SaveChanges();
            }

            foreach (var entity in _context.Appointments)
            {
                _context.Appointments.Remove(entity);
                _context.SaveChanges();
            }

            foreach (var entity in _context.Clinics)
            {
                _context.Clinics.Remove(entity);
                _context.SaveChanges();
            }

            foreach (var entity in _context.Diseases)
            {
                _context.Diseases.Remove(entity);
                _context.SaveChanges();
            }

            foreach (var entity in _context.PatientCard)
            {
                _context.PatientCard.Remove(entity);
                _context.SaveChanges();
            }

            _context.SaveChanges();
        }
    }

    internal IUnitOfWork CreateUnitOfWork(bool includeClientGroupToContext)
    {
        _contextAccessor.Setup(c => c.HttpContext.User.Claims)
            .Returns(new List<Claim>
            {
                    new(Consts.ClinicIdClaim, ClinicId.ToString())
            });

        if (includeClientGroupToContext)
        {
            _contextAccessor.Setup(c => c.HttpContext.User.HasClaim(It.IsAny<Predicate<Claim>>()))
                .Returns(true);
        }

        var inMemorySettings = new Dictionary<string, string>();

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

        var mockServiceProvider = new Mock<IServiceProvider>(); 
        return new UnitOfWork(mockServiceProvider.Object);
    }
}

