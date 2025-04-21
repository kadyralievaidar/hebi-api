using Hebi_Api.Features.Core.DataAccess.Interfaces;
using System.Data;

namespace Hebi_Api.Features.Core.DataAccess.UOW;

public interface IUnitOfWork
{
    /// <summary>
    ///     Execute queryable.
    ///     Asynchronously creates a <see cref="List{TModel}" />
    ///     from an <see cref="IQueryable{TModel}" /> by enumerating it asynchronously.
    /// </summary>
    /// <typeparam name="TModel">Type of model</typeparam>
    /// <param name="rows">An <see cref="IQueryable{TModel}" /> to create a list from.</param>
    /// <returns>The task result contains a <see cref="List{TModel}" /> that contains elements from the input sequence.</returns>
    Task<List<TModel>> ExecuteAsync<TModel>(IQueryable<TModel> rows);

    /// <summary>
    ///     User repository
    /// </summary>
    IUsersRepository UsersRepository { get; }

    /// <summary>
    ///     Disease repository
    /// </summary>
    IDiseasesRepository DiseaseRepository { get; }

    /// <summary>
    ///     Appointment repository
    /// </summary>
    IAppointmentsRepository AppointmentRepository { get; }

    /// <summary>
    ///     Patient card repository
    /// </summary>
    IUserCardsRepository UserCardsRepository { get; }

    /// <summary>
    ///    Clinic repository
    /// </summary>
    IClinicsRepository ClinicRepository { get; }

    /// <summary>
    ///     Shifts repository
    /// </summary>
    IShiftsRepository ShiftsRepository { get; }

    /// <summary>
    ///     Save changes
    /// </summary>
    void Save();

    /// <summary>
    ///     Save changes asynchronously
    /// </summary>
    Task SaveAsync();

    /// <summary>
    ///     Clear Change Tracker
    /// </summary>
    void ClearChangeTracker();

    /// <summary>
    ///     Begin transaction
    /// </summary>
    /// <param name="isolationLevel">Isolation level</param>
    /// <returns>Transaction</returns>
    IDbTransaction BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
}
