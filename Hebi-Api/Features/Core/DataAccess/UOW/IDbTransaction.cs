using System.Data;

namespace Hebi_Api.Features.Core.DataAccess.UOW;

/// <summary>
///     Transaction database
/// </summary>
public interface IDbTransaction : IDisposable
{
    /// <summary>
    ///     Begin transaction
    /// </summary>
    /// <param name="isolationLevel">Isolation level</param>
    void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);

    /// <summary>
    ///     Commit transaction
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Execution result</returns>
    Task CommitAsync(CancellationToken cancellationToken = default);

    /// <summary>
    ///     Rollback transaction
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Execution result</returns>
    Task RollbackAsync(CancellationToken cancellationToken = default);
}
