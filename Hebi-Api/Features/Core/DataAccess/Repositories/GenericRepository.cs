using Hebi_Api.Features.Core.Common;
using Hebi_Api.Features.Core.Common.Interfaces;
using Hebi_Api.Features.Core.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Hebi_Api.Features.Core.DataAccess.Repositories;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class, new()
{
    protected readonly IHttpContextAccessor ContextAccessor;

    public GenericRepository(HebiDbContext context, IHttpContextAccessor contextAccessor)
    {
        Context = context;
        DbSet = context.Set<TEntity>();
        ContextAccessor = contextAccessor;
    }

    /// <summary>
    ///     Database context
    /// </summary>
    public HebiDbContext Context { get; }


    /// <summary>
    ///     Returns context with default filters
    /// </summary>
    /// <returns></returns>
    protected IQueryable<TEntity> GetContext()
    {
        var context = Context.Set<TEntity>().AsQueryable();
        context = FilterByClinicId(context);

        return context;
    }
    /// <summary>
    ///     DbSet
    /// </summary>
    public DbSet<TEntity> DbSet { get; }

    /// <summary>
    ///     Apply pagination
    /// </summary>
    /// <typeparam name="TSource">Type of source</typeparam>
    /// <param name="rows">Rows to pagination</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="pageNumber">Page number</param>
    /// <returns>Pagination data</returns>
    public async Task<(List<TSource> PaginatedCollection, int TotalCount)> ApplyPagination<TSource>(IQueryable<TSource> rows, int pageSize, int pageNumber)
    {
        var paginatedCollection = await rows.Skip(pageSize * pageNumber).Take(pageSize).ToListAsync();
        return (paginatedCollection, rows.Count());
    }

    /// <summary>
    ///     Apply pagination
    /// </summary>
    /// <typeparam name="TSource">Type of source</typeparam>
    /// <param name="rows">Rows to pagination</param>
    /// <param name="pagination">Navigation information</param>
    /// <returns>Pagination data</returns>
    public async Task<(List<TSource> PaginatedCollection, int TotalCount)> ApplyPagination<TSource>(IQueryable<TSource> rows, IPaginationRequest pagination)
        => await ApplyPagination(rows, pagination.PageSize, pagination.PageNumber);

    /// <summary>
    ///     Return a list of entities. Similar to LINQ Where
    /// </summary>
    /// <param name="filter">Filter for selection condition</param>
    /// <returns>Entities</returns>
    public virtual IEnumerable<TEntity> Where(Expression<Func<TEntity, bool>>? filter = null)
    {
        IQueryable<TEntity> query = DbSet;
        query = Filter(query, filter);
        return query.ToList();
    }

    /// <summary>
    ///     Return a list of entities. Similar to LINQ Where
    /// </summary>
    /// <param name="filter">Filter for selection condition</param>
    /// <returns>Entities</returns>
    public virtual async Task<IEnumerable<TEntity>> WhereAsync(Expression<Func<TEntity, bool>>? filter = null)
    {
        IQueryable<TEntity> query = DbSet;
        query = Filter(query, filter);
        return await query.ToListAsync();
    }

    /// <summary>
    ///     Return a list of entities. Similar to LINQ Where
    /// </summary>
    /// <param name="filter">Filter for selection condition</param>
    /// <returns>Entities</returns>
    public virtual IQueryable<TEntity> WhereLazy(Expression<Func<TEntity, bool>>? filter = null)
    {
        IQueryable<TEntity> query = DbSet;
        query = Filter(query, filter);
        return query;
    }

    /// <summary>
    ///     As queryable
    /// </summary>
    public virtual IQueryable<TEntity> AsQueryable() => DbSet;

    /// <summary>
    ///     Return an entity. Similar to LINQ FirstOrDefault
    /// </summary>
    /// <param name="filter">Filter for selection condition</param>
    /// <returns>Entity</returns>
    public virtual TEntity? FirstOrDefault(Expression<Func<TEntity, bool>>? filter = null)
    {
        IQueryable<TEntity> query = DbSet;
        query = Filter(query, filter);
        return query.FirstOrDefault();
    }

    /// <summary>
    ///     Return an entity. Similar to LINQ FirstOrDefault
    /// </summary>
    /// <param name="filter">Filter for selection condition</param>
    /// <returns>Entity</returns>
    public virtual async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>>? filter = null)
    {
        IQueryable<TEntity> query = DbSet;
        query = Filter(query, filter);
        return await query.FirstOrDefaultAsync();
    }

    /// <summary>
    ///     Get an entity by primary keys
    /// </summary>
    /// <param name="ids">Primary keys</param>
    /// <returns>Entity</returns>
    public virtual TEntity? GetById(params object[]? ids) => ids == null ? null : DbSet.Find(ids);

    /// <summary>
    ///     Get an entity by primary keys
    /// </summary>
    /// <param name="ids">Primary keys</param>
    /// <returns>Entity</returns>
    public virtual async Task<TEntity?> GetByIdAsync(params object[]? ids) => ids == null ? null : await DbSet.FindAsync(ids);

    /// <summary>
    ///     Insert a new entity into the context
    /// </summary>
    /// <param name="entity">Entity to insert</param>
    public virtual void Insert(TEntity entity) => DbSet.Add(entity);

    /// <summary>
    ///     Insert a new entity into the context
    /// </summary>
    /// <param name="entity">Entity to insert</param>
    public virtual async Task InsertAsync(TEntity entity) => await DbSet.AddAsync(entity);

    /// <summary>
    ///     Insert an array of new entities into the context
    /// </summary>
    /// <param name="entities">Entities to insert</param>
    public virtual void InsertRange(IEnumerable<TEntity> entities) => DbSet.AddRange(entities);

    /// <summary>
    ///     Insert an array of new entities into the context
    /// </summary>
    /// <param name="entities">Entities to insert</param>
    public virtual async Task InsertRangeAsync(IEnumerable<TEntity> entities) => await DbSet.AddRangeAsync(entities);

    /// <summary>
    ///     Delete an entity
    /// </summary>
    /// <param name="entityToDelete">Entity to delete</param>
    public virtual void Delete(TEntity entityToDelete)
    {
        if (Context.Entry(entityToDelete).State == EntityState.Detached)
            DbSet.Attach(entityToDelete);

        DbSet.Remove(entityToDelete);
    }

    /// <summary>
    ///     Remove a list of entities
    /// </summary>
    /// <param name="toDelete">Entities</param>
    public virtual void DeleteRange(IEnumerable<TEntity> toDelete) => DbSet.RemoveRange(toDelete);

    /// <summary>
    ///     Update an entity in the repository
    /// </summary>
    /// <param name="entityToUpdate">Entity to update</param>
    public virtual void Update(TEntity entityToUpdate)
    {
        Context.Entry(entityToUpdate).State = EntityState.Detached;
        DbSet.Attach(entityToUpdate);
        Context.Entry(entityToUpdate).State = EntityState.Modified;
    }

    /// <summary>
    ///     Reload entity
    /// </summary>
    /// <param name="entity">Entity</param>
    public virtual void Reload(TEntity entity) => Context.Entry(entity).Reload();

    /// <summary>
    ///     Presence of records by condition
    /// </summary>
    /// <param name="predicate">Filter for selection condition</param>
    public virtual bool Any(Expression<Func<TEntity, bool>> predicate) => DbSet.Any(predicate);

    /// <summary>
    ///     Select operator
    /// </summary>
    /// <typeparam name="T">Field type</typeparam>
    /// <param name="selector">Filter for selection condition</param>
    public virtual IEnumerable<T> Select<T>(Expression<Func<TEntity, T>> selector) => DbSet.Select(selector);

    /// <summary>
    ///     Sort ascending
    /// </summary>
    /// <typeparam name="T">Field type</typeparam>
    /// <param name="selector">Filter for selection condition</param>
    public virtual IOrderedQueryable<TEntity> OrderBy<T>(Expression<Func<TEntity, T>> selector) => DbSet.OrderBy(selector);

    /// <summary>
    ///     Sort descending
    /// </summary>
    /// <typeparam name="T">Field type</typeparam>
    /// <param name="selector">Filter for selection condition</param>
    public virtual IOrderedQueryable<TEntity> OrderByDescending<T>(Expression<Func<TEntity, T>> selector) => DbSet.OrderByDescending(selector);

    /// <summary>
    ///     Count of elements
    /// </summary>
    public virtual int Count() => DbSet.Count();

    /// <summary>
    ///     Sum by condition
    /// </summary>
    /// <param name="selector">Filter for selection condition</param>
    public virtual decimal Sum(Expression<Func<TEntity, decimal>> selector) => DbSet.Sum(selector);

    /// <summary>
    ///     Filter entities
    /// </summary>
    /// <param name="data">IQueryable data</param>
    /// <param name="filter">Filter for selection condition</param>
    /// <returns>IQueryable data</returns>
    protected virtual IQueryable<TEntity> Filter(IQueryable<TEntity> data, Expression<Func<TEntity, bool>>? filter) => filter == null ? data : data.Where(filter);

    private Guid? FilterByClinicId()
    {
        var claim = ContextAccessor.HttpContext?.User?.Claims.FirstOrDefault(c =>
            c.Type == Consts.ClinicIdClaim);

        if (IsUserSuperAdmin())
        {
            if (claim != null)
            {
                if (!string.IsNullOrEmpty(claim.Value))
                    return new Guid(claim.Value);

                return null;
            }

            return null;
        }
        else if (IsRegularUser())
        {
            if (claim != null) return new Guid(claim.Value);
        }

        throw new InvalidOperationException("ClientGroup is not specified for user");
    }
}
