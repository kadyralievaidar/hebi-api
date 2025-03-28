using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Hebi_Api.Features.Core.DataAccess.UOW;

/// <summary>
///     Transaction database
/// </summary>
internal class DbTransaction : IDbTransaction
{
    private readonly HebiDbContext _context;
    private IDbContextTransaction? _transaction;
    private long _countActiveTransaction;

    public DbTransaction(HebiDbContext context)
    {
        _context = context;
    }

    /// <summary>
    ///     Begin transaction
    /// </summary>
    /// <param name="isolationLevel">Isolation level</param>
    public void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
    {
        if (Interlocked.Read(ref _countActiveTransaction) == 0)
            _transaction = _context.Database.BeginTransaction(isolationLevel);

        Interlocked.Increment(ref _countActiveTransaction);
    }

    /// <summary>
    ///     Commit transaction
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Execution result</returns>
    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction is null)
            throw new InvalidOperationException();

        Interlocked.Decrement(ref _countActiveTransaction);

        if (Interlocked.Read(ref _countActiveTransaction) == 0)
            await _transaction.CommitAsync(cancellationToken);
    }

    /// <summary>
    ///     Rollback transaction
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Execution result</returns>
    public async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction is null)
            throw new InvalidOperationException();

        Interlocked.Decrement(ref _countActiveTransaction);

        if (Interlocked.Read(ref _countActiveTransaction) <= 0)
            await _transaction.RollbackAsync(cancellationToken);
    }

    ~DbTransaction()
    {
        Dispose(false);
    }

    private void Dispose(bool disposing)
    {
        if (disposing && Interlocked.Read(ref _countActiveTransaction) <= 0)
            _transaction?.Dispose();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}