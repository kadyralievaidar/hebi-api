using Hebi_Api.Features.Core.DataAccess.Interfaces;
using Hebi_Api.Features.Core.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Hebi_Api.Features.Core.DataAccess.UOW;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IDbTransaction _dbTransaction;
    private readonly Lazy<IUsersRepository> _usersRepository;
    private readonly Lazy<IDiseasesRepository> _diseaseRepository;
    private readonly Lazy<IAppointmentsRepository> _appointmentRepository;
    private readonly Lazy<IUserCardsRepository> _userCardsRepository;
    private readonly Lazy<IClinicsRepository> _clinicRepository;
    private readonly Lazy<IShiftsRepository> _shiftsRepository;


    /// <summary>
    ///     Database context
    /// </summary>
    private readonly HebiDbContext _context;

    /// <inheritdoc />
    public UnitOfWork(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _dbTransaction = serviceProvider.GetRequiredService<IDbTransaction>();
        _context = serviceProvider.GetRequiredService<HebiDbContext>();
        _usersRepository = new Lazy<IUsersRepository>(serviceProvider.GetRequiredService<IUsersRepository>());
        _diseaseRepository = new Lazy<IDiseasesRepository>(serviceProvider.GetRequiredService<IDiseasesRepository>());
        _appointmentRepository = new Lazy<IAppointmentsRepository>(serviceProvider.GetRequiredService<IAppointmentsRepository>());
        _userCardsRepository = new Lazy<IUserCardsRepository>(serviceProvider.GetRequiredService<IUserCardsRepository>());
        _clinicRepository = new Lazy<IClinicsRepository>(serviceProvider.GetRequiredService<IClinicsRepository>());
        _shiftsRepository = new Lazy<IShiftsRepository>(serviceProvider.GetRequiredService<IShiftsRepository>());
    }

    ~UnitOfWork()
    {
        Dispose(false);
    }

    /// <summary>
    ///     Save сhanges
    /// </summary>
    public void Save()
    {
        PrepareContextForSaving();
        _context.SaveChanges();
    }

    /// <summary>
    ///     Save changes asynchronously
    /// </summary>
    public async Task SaveAsync()
    {
        PrepareContextForSaving();
        await _context.SaveChangesAsync();
    }

    /// <summary>
    ///     Clear Change Tracker
    /// </summary>
    public void ClearChangeTracker()
        => _context.ChangeTracker.Clear();

    /// <summary>
    ///     Begin transaction
    /// </summary>
    /// <param name="isolationLevel">Isolation level</param>
    /// <returns>Transaction</returns>
    public IDbTransaction BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
    {
        _dbTransaction.BeginTransaction(isolationLevel);
        return _dbTransaction;
    }

    /// <summary>
    ///     Execute queryable.
    ///     Asynchronously creates a <see cref="List{TModel}" />
    ///     from an <see cref="IQueryable{TModel}" /> by enumerating it asynchronously.
    /// </summary>
    /// <typeparam name="TModel">Type of model</typeparam>
    /// <param name="rows">An <see cref="IQueryable{TModel}" /> to create a list from.</param>
    /// <returns>The task result contains a <see cref="List{TModel}" /> that contains elements from the input sequence.</returns>
    public async Task<List<TModel>> ExecuteAsync<TModel>(IQueryable<TModel> rows) => await rows.ToListAsync();

    public IUsersRepository UsersRepository => _usersRepository.Value;
    public IDiseasesRepository DiseaseRepository => _diseaseRepository.Value;
    public IAppointmentsRepository AppointmentRepository => _appointmentRepository.Value;
    public IUserCardsRepository UserCardsRepository => _userCardsRepository.Value;
    public IClinicsRepository ClinicRepository => _clinicRepository.Value;
    public IShiftsRepository ShiftsRepository => _shiftsRepository.Value;

    /// <summary>
    ///     Prepare context for saving
    /// </summary>
    private void PrepareContextForSaving()
    {
        var modifiedPersistentsEntities = _context.ChangeTracker.Entries()
            .Where(x => x.State == EntityState.Modified)
            .Select(s => s.Entity)
            .OfType<IBaseModel>()
            .ToList();

        var addedPersistentsEntities = _context.ChangeTracker.Entries()
            .Where(x => x.State == EntityState.Added)
            .Select(s => s.Entity)
            .OfType<IBaseModel>()
            .ToList();

        foreach (var persistentObject in modifiedPersistentsEntities)
            persistentObject.LastModifiedAt = DateTime.UtcNow;

        foreach (var persistentObject in addedPersistentsEntities)
        {
            persistentObject.LastModifiedAt = DateTime.UtcNow;
            persistentObject.CreatedAt = DateTime.UtcNow;
        }
    }

    private void Dispose(bool disposing)
    {
        if (disposing)
            _context.Dispose();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
