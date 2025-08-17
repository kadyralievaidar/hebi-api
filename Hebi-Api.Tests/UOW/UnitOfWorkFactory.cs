using Hebi_Api.Features.Core.Common;
using Hebi_Api.Features.Core.DataAccess;
using Hebi_Api.Features.Core.DataAccess.Interfaces;
using Hebi_Api.Features.Core.DataAccess.Models;
using Hebi_Api.Features.Core.DataAccess.Repositories;
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
        var clinic = new Clinic() { Id = TestHelper.ClinicId };
        AddData(new List<Clinic>() { clinic });
        var admin = new ApplicationUser()
        {
            UserName = "Test",
            Id = TestHelper.DoctorId,
            FirstName = "Test",
            LastName = "Test",
            Email = "test@test.com",
            NormalizedUserName = "Test"
        };
        AddData(new List<ApplicationUser>() { admin });
        DetachForReload(clinic);
    }

    /// <inheritdoc />
    public void Dispose()
    {
        _connection?.Dispose();
        _context.Database.EnsureDeleted();
        _context?.Dispose();
    }

    internal void AddData<T>(List<T> data) where T : class, IBaseModel
    {
        var type = typeof(DbContextMock);
        var prop = type.GetProperties().FirstOrDefault(x => x.PropertyType == typeof(DbSet<T>));
        var getter = prop!.GetGetMethod();

        var dbSet = (DbSet<T>)getter!.Invoke(_context, null)!;
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
        _contextAccessor.Setup(c => c.HttpContext!.User.Claims)
            .Returns(new List<Claim>
            {
                    new(Consts.ClinicIdClaim, TestHelper.ClinicId.ToString()),
                    new(Consts.UserId, Guid.NewGuid().ToString())
            });

        if (includeClientGroupToContext)
        {
            _contextAccessor.Setup(c => c.HttpContext!.User.HasClaim(It.IsAny<Predicate<Claim>>()))
                .Returns(true);
        }

        var inMemorySettings = new Dictionary<string, string>();

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings!)
            .Build();

        var mockServiceProvider = new Mock<IServiceProvider>();
        mockServiceProvider.Setup(x => x.GetService(typeof(IDbTransaction))).Returns(new Mock<IDbTransaction>().Object);
        mockServiceProvider.Setup(x => x.GetService(typeof(IAppointmentsRepository))).Returns(new AppointmentsRepository(_context, _contextAccessor.Object));
        mockServiceProvider.Setup(x => x.GetService(typeof(IClinicsRepository))).Returns(new ClinicsRepository(_context, _contextAccessor.Object));
        mockServiceProvider.Setup(x => x.GetService(typeof(IDiseasesRepository))).Returns(new DiseasesRepository(_context, _contextAccessor.Object));
        mockServiceProvider.Setup(x => x.GetService(typeof(IShiftsRepository))).Returns(new ShiftsRepository(_context, _contextAccessor.Object));
        mockServiceProvider.Setup(x => x.GetService(typeof(IUserCardsRepository))).Returns(new UserCardsRepository(_context, _contextAccessor.Object));
        mockServiceProvider.Setup(x => x.GetService(typeof(IUsersRepository))).Returns(new UsersRepository(_context, _contextAccessor.Object));
        mockServiceProvider.Setup(x => x.GetService(typeof(IShiftTemplateRepository))).Returns(new ShiftTemplateRepository(_context, _contextAccessor.Object));
        mockServiceProvider.Setup(x => x.GetService(typeof(HebiDbContext))).Returns(_context);
        return new UnitOfWork(mockServiceProvider.Object);
    }

    internal DbContextMock GetDbContext()
    {
        return _context;
    }
    public void DetachForReload(IBaseModel entity)
    {
        try
        {
            _context.Entry(entity).State = EntityState.Detached;
        }
        catch (Exception)
        {
        }
    }
}

