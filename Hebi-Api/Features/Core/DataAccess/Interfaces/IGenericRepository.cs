using System.ComponentModel;
using System.Linq.Expressions;
using Hebi_Api.Features.Core.Common.Interfaces;
using Hebi_Api.Features.Core.DataAccess.Models;

namespace Hebi_Api.Features.Core.DataAccess.Interfaces;

/// <summary>
///     Generic repository
/// </summary>
/// <typeparam name="TEntity">Entity type</typeparam>
public interface IGenericRepository<TEntity> where TEntity : class, IBaseModel
{
    /// <summary>
    ///     Apply pagination
    /// </summary>
    /// <typeparam name="TSource">Type of source</typeparam>
    /// <param name="rows">Rows to pagination</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="pageNumber">Page number</param>
    /// <returns>Pagination data</returns>
    Task<(List<TSource> PaginatedCollection, int TotalCount)> ApplyPagination<TSource>(IQueryable<TSource> rows, int pageSize, int pageNumber);

    /// <summary>
    ///     Apply pagination
    /// </summary>
    /// <typeparam name="TSource">Type of source</typeparam>
    /// <param name="rows">Rows to pagination</param>
    /// <param name="pagination">Navigation information</param>
    /// <returns>Pagination data</returns>
    Task<(List<TSource> PaginatedCollection, int TotalCount)> ApplyPagination<TSource>(IQueryable<TSource> rows, IPaginationRequest pagination);

    /// <summary>
    ///     Return a list of entities. Similar to LINQ Where
    /// </summary>
    /// <param name="filter">Filter for selection condition</param>
    /// <returns>Entities</returns>
    IEnumerable<TEntity> Where(Expression<Func<TEntity, bool>>? filter = null);

    /// <summary>
    ///     Return a list of entities. Similar to LINQ Where
    /// </summary>
    /// <param name="filter">Filter for selection condition</param>
    /// <returns>Entities</returns>
    IQueryable<TEntity> WhereLazy(Expression<Func<TEntity, bool>>? filter = null);

    /// <summary>
    ///     As queryable
    /// </summary>
    IQueryable<TEntity> AsQueryable();

    /// <summary>
    ///     Return a list of entities. Similar to LINQ Where
    /// </summary>
    /// <param name="filter">Filter for selection condition</param>
    /// <returns>Entities</returns>
    Task<IEnumerable<TEntity>> WhereAsync(Expression<Func<TEntity, bool>>? filter = null);

    /// <summary>
    ///     Return an entity. Similar to LINQ FirstOrDefault
    /// </summary>
    /// <param name="filter">Filter for selection condition</param>
    /// <returns>Entity</returns>
    TEntity? FirstOrDefault(Expression<Func<TEntity, bool>>? filter = null);

    /// <summary>
    ///     Return an entity. Similar to LINQ FirstOrDefault
    /// </summary>
    /// <param name="filter">Filter for selection condition</param>
    /// <returns>Entity</returns>
    Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>>? filter = null, List<string>? relations = null);

    /// <summary>
    ///     Get an entity by primary keys
    /// </summary>
    /// <param name="id">Primary keys</param>
    /// <returns>Entity</returns>
    TEntity? GetById(Guid id);

    /// <summary>
    ///     Get an entity by primary keys
    /// </summary>
    /// <param name="id">Primary keys</param>
    /// <param name="relations">Primary keys</param>
    /// <returns>Entity</returns>
    Task<TEntity?> GetByIdAsync(Guid id, List<string>? relations = null);

    /// <summary>
    ///     Insert a new entity into the context
    /// </summary>
    /// <param name="entity">Entity to insert</param>
    void Insert(TEntity entity);

    /// <summary>
    ///     Insert a new entity into the context
    /// </summary>
    /// <param name="entity">Entity to insert</param>
    Task InsertAsync(TEntity entity);

    /// <summary>
    ///     Insert an array of new entities into the context
    /// </summary>
    /// <param name="entities">Entities to insert</param>
    void InsertRange(IEnumerable<TEntity> entities);

    /// <summary>
    ///     Insert an array of new entities into the context
    /// </summary>
    /// <param name="entities">Entities to insert</param>
    Task InsertRangeAsync(IEnumerable<TEntity> entities);

    /// <summary>
    ///     Delete an entity
    /// </summary>
    /// <param name="entityToDelete">Entity to delete</param>
    void Delete(TEntity entityToDelete);

    /// <summary>
    ///     Remove a list of entities
    /// </summary>
    /// <param name="toDelete">Entities</param>
    void DeleteRange(IEnumerable<TEntity> toDelete);

    /// <summary>
    ///     Update an entity in the repository
    /// </summary>
    /// <param name="entityToUpdate">Entity to update</param>
    void Update(TEntity entityToUpdate);

    /// <summary>
    ///     Reload entity
    /// </summary>
    /// <param name="entity">Entity</param>
    void Reload(TEntity entity);

    /// <summary>
    ///     Presence of records by condition
    /// </summary>
    /// <param name="predicate">Filter for selection condition</param>
    bool Any(Expression<Func<TEntity, bool>> predicate);

    /// <summary>
    ///     Select operator
    /// </summary>
    /// <typeparam name="T">Field type</typeparam>
    /// <param name="selector">Filter for selection condition</param>
    IEnumerable<T> Select<T>(Expression<Func<TEntity, T>> selector);

    /// <summary>
    ///     Sort ascending
    /// </summary>
    /// <typeparam name="T">Field type</typeparam>
    /// <param name="selector">Filter for selection condition</param>
    IOrderedQueryable<TEntity> OrderBy<T>(Expression<Func<TEntity, T>> selector);

    /// <summary>
    ///     Sort descending
    /// </summary>
    /// <typeparam name="T">Field type</typeparam>
    /// <param name="selector">Filter for selection condition</param>
    IOrderedQueryable<TEntity> OrderByDescending<T>(Expression<Func<TEntity, T>> selector);

    /// <summary>
    ///     Count of elements
    /// </summary>
    int Count();

    /// <summary>
    ///     Sum by condition
    /// </summary>
    /// <param name="selector">Filter for selection condition</param>
    decimal Sum(Expression<Func<TEntity, decimal>> selector);


    /// <summary>
    ///     Search paged entities
    /// </summary>
    /// <param name="skip">how much to skip</param>
    /// <param name="take">how much to take</param>
    /// <param name="predicate">expression</param>
    /// <param name="sortBy">sorting</param>
    /// <param name="sortDirection">sort direction</param>
    /// <returns></returns>
    Task<List<TEntity>> SearchAsync(Expression<Func<TEntity, bool>> predicate, string? sortBy = null,
        ListSortDirection? sortDirection = null, int? skip = null, int? take = null);

    /// <summary>
    ///     GetAllAsync
    /// </summary>
    /// <returns></returns>
    Task<List<TEntity>> GetAllAsync();

    Task<bool> ExistAsync(Guid id);

    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);

    /// <summary>
    ///     Update range
    /// </summary>
    /// <param name="entities"></param>
    /// <returns></returns>
    Task UpdateRangeAsync(IEnumerable<TEntity> entities);

    IQueryable<TEntity> SearchQuery(Expression<Func<TEntity, bool>> predicate, string? sortBy = null,
        ListSortDirection? sortDirection = null,
        int? skip = null, int? take = null, List<string>? relations = null);
}