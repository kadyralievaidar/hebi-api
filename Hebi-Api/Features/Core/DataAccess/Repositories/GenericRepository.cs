using Hebi_Api.Features.Core.Common.Interfaces;
using Hebi_Api.Features.Core.DataAccess.Interfaces;
using Hebi_Api.Features.Core.DataAccess.Models;
using Hebi_Api.Features.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace Hebi_Api.Features.Core.DataAccess.Repositories;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class, IBaseModel
{
    protected readonly IHttpContextAccessor ContextAccessor;

    /// <summary>
    ///     Context
    /// </summary>
    protected readonly HebiDbContext Context;

    public GenericRepository(HebiDbContext context, IHttpContextAccessor contextAccessor)
    {
        Context = context;
        ContextAccessor = contextAccessor;
    }

    protected virtual IQueryable<TEntity> SetWithRelatedEntities => GetContext();

    protected virtual IQueryable<TEntity> SetWithRelatedEntitiesAsNoTracking => GetContext().AsNoTracking();

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
    protected virtual IQueryable<TEntity> Queryable => GetContext().AsNoTracking();

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
        IQueryable<TEntity> query = Queryable;
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
        IQueryable<TEntity> query = Queryable;
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
        IQueryable<TEntity> query = Queryable;
        query = Filter(query, filter);
        return query;
    }

    /// <summary>
    ///     As queryable
    /// </summary>
    public virtual IQueryable<TEntity> AsQueryable() => Queryable;

    /// <summary>
    ///     Return an entity. Similar to LINQ FirstOrDefault
    /// </summary>
    /// <param name="filter">Filter for selection condition</param>
    /// <returns>Entity</returns>
    public virtual TEntity? FirstOrDefault(Expression<Func<TEntity, bool>>? filter = null)
    {
        IQueryable<TEntity> query = Queryable;
        query = Filter(query, filter);
        return query.FirstOrDefault();
    }

    /// <summary>
    ///     Return an entity. Similar to LINQ FirstOrDefault
    /// </summary>
    /// <param name="filter">Filter for selection condition</param>
    /// <returns>Entity</returns>
    public virtual async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>>? filter = null, List<string>? relations = null)
    {
        var context = relations != null && RelationsAreValid(relations)
                    ? GetTrackingContextWithRelations(relations)
                    : SetWithRelatedEntities;
        context = Filter(context, filter);
        return await context.FirstOrDefaultAsync();
    }

    /// <summary>
    ///     Get an entity by primary keys
    /// </summary>
    /// <param name="ids">Primary keys</param>
    /// <returns>Entity</returns>
    public virtual TEntity? GetById(Guid id)
    {
        return Queryable.SingleOrDefault(x => x.Id == id);
    }

    /// <summary>
    ///     Get an entity by primary keys
    /// </summary>
    /// <param name="ids">Primary keys</param>
    /// <returns>Entity</returns>
    public virtual async Task<TEntity?> GetByIdAsync(Guid id, List<string>? relations = null)
    {
        var context = relations != null && RelationsAreValid(relations) ?
                GetNoTrackingContextWithRelations(relations)
                : SetWithRelatedEntitiesAsNoTracking;

        return await context.SingleOrDefaultAsync(x => x.Id == id);
    }
    /// <summary>
    ///     Insert a new entity into the context
    /// </summary>
    /// <param name="entity">Entity to insert</param>
    public virtual void Insert(TEntity entity) => Context.Add(entity);

    /// <summary>
    ///     Insert a new entity into the context
    /// </summary>
    /// <param name="entity">Entity to insert</param>
    public virtual async Task InsertAsync(TEntity entity)
    {
        entity.ClinicId = ContextAccessor.GetClinicId();
        entity.CreatedAt = DateTime.UtcNow;
        entity.CreatedBy = ContextAccessor.GetUserIdentifier();
        await Context.AddAsync(entity);
    }

    /// <summary>
    ///     Insert an array of new entities into the context
    /// </summary>
    /// <param name="entities">Entities to insert</param>
    public virtual void InsertRange(IEnumerable<TEntity> entities) => Context.AddRange(entities);

    /// <summary>
    ///     Insert an array of new entities into the context
    /// </summary>
    /// <param name="entities">Entities to insert</param>
    public virtual async Task InsertRangeAsync(IEnumerable<TEntity> entities) => await Context.AddRangeAsync(entities);

    /// <summary>
    ///     Delete an entity
    /// </summary>
    /// <param name="entityToDelete">Entity to delete</param>
    public virtual void Delete(TEntity entityToDelete)
    {
        if (Context.Entry(entityToDelete).State == EntityState.Detached)
            Context.Attach(entityToDelete);

        Context.Remove(entityToDelete);
    }

    /// <summary>
    ///     Remove a list of entities
    /// </summary>
    /// <param name="toDelete">Entities</param>
    public virtual void DeleteRange(IEnumerable<TEntity> toDelete) => Context.RemoveRange(toDelete);

    /// <summary>
    ///     Update an entity in the repository
    /// </summary>
    /// <param name="entityToUpdate">Entity to update</param>
    public virtual void Update(TEntity entityToUpdate)
    {
        Context.Entry(entityToUpdate).State = EntityState.Detached;
        Context.Attach(entityToUpdate);
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
    public virtual bool Any(Expression<Func<TEntity, bool>> predicate) => Queryable.Any(predicate);

    /// <summary>
    ///     Select operator
    /// </summary>
    /// <typeparam name="T">Field type</typeparam>
    /// <param name="selector">Filter for selection condition</param>
    public virtual IEnumerable<T> Select<T>(Expression<Func<TEntity, T>> selector) => Queryable.Select(selector);

    /// <summary>
    ///     Sort ascending
    /// </summary>
    /// <typeparam name="T">Field type</typeparam>
    /// <param name="selector">Filter for selection condition</param>
    public virtual IOrderedQueryable<TEntity> OrderBy<T>(Expression<Func<TEntity, T>> selector) => Queryable.OrderBy(selector);

    /// <summary>
    ///     Sort descending
    /// </summary>
    /// <typeparam name="T">Field type</typeparam>
    /// <param name="selector">Filter for selection condition</param>
    public virtual IOrderedQueryable<TEntity> OrderByDescending<T>(Expression<Func<TEntity, T>> selector) => Queryable.OrderByDescending(selector);

    /// <summary>
    ///     Count of elements
    /// </summary>
    public virtual int Count() => Queryable.Count();

    /// <summary>
    ///     Sum by condition
    /// </summary>
    /// <param name="selector">Filter for selection condition</param>
    public virtual decimal Sum(Expression<Func<TEntity, decimal>> selector) => Queryable.Sum(selector);

    /// <summary>
    ///     Filter entities
    /// </summary>
    /// <param name="data">IQueryable data</param>
    /// <param name="filter">Filter for selection condition</param>
    /// <returns>IQueryable data</returns>
    protected virtual IQueryable<TEntity> Filter(IQueryable<TEntity> data, Expression<Func<TEntity, bool>>? filter) => filter == null ? data : data.Where(filter);

    public virtual async Task<List<TEntity>> SearchAsync(Expression<Func<TEntity, bool>> predicate, string? sortBy = null,
    ListSortDirection? sortDirection = null, int? skip = null,
    int? take = null)
    {
        if (string.IsNullOrEmpty(sortBy))
            return await Queryable.Where(predicate).TrySkip(skip ?? 0)
                .TryTake(take ?? int.MaxValue).ToListAsync();

        //Checks if type has the sorting property ignoring case
        var propertyInfo = typeof(TEntity).GetProperties().SingleOrDefault(p => p.Name.ToLower() == sortBy.ToLower());

        if (propertyInfo == null)
            throw new ArgumentException(
                $"Entity {typeof(TEntity).FullName} doesn't have the property {sortBy} for sorting");

        //Ensure correct casing for property sorting
        sortBy = propertyInfo.Name;

        return await Queryable.Where(predicate)
            .OrderByDynamic(sortBy, (sortDirection ?? ListSortDirection.Ascending) == ListSortDirection.Descending).TrySkip(skip)
            .TryTake(take).ToListAsync();
    }

    protected IQueryable<TEntity> FilterByClinicId(IQueryable<TEntity> query)
    {
        var clientGroupId = ContextAccessor.GetClinicId(); 
        query = query.Where(c => c.ClinicId.Equals(clientGroupId));
        return query;
    }

    /// <summary>
    ///     
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    public virtual async Task<List<TEntity>> GetAllAsync() => await Queryable.ToListAsync();

    public async Task<bool> ExistAsync(Guid id)
    {
        return await Queryable.AnyAsync(x => x.Id == id);
    }

    public virtual async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await Queryable.AnyAsync(predicate);
    }

    public Task UpdateRangeAsync(IEnumerable<TEntity> entities)
    {
        Context.UpdateRange(entities);
        return Task.CompletedTask;
    }

    private IQueryable<TEntity> GetTrackingContextWithRelations(IEnumerable<string> relations)
    {
        var context = SetWithRelatedEntities;
        context = relations.Aggregate(context, (current, relation) => current.Include(relation));
        return context;
    }

    /// <inheritdoc />
    public List<TEntity> GetByIdsWithTracking(IEnumerable<Guid> ids, List<string>? relations = null)
    {
        var context = relations != null && RelationsAreValid(relations)
            ? GetTrackingContextWithRelations(relations)
            : SetWithRelatedEntities;
        return context.Where(e => ids.Contains(e.Id)).ToList();
    }

    private static bool RelationsAreValid(List<string> relations)
    {
        foreach (var relation in relations) TypeHasRelationProperty(typeof(TEntity), relation);
        return true;
    }

    private static bool TypeHasRelationProperty(Type? type, string relation)
    {
        if (relation.Contains('.'))
        {
            var propertyInfo = type?.GetProperty(relation.Split('.')[0]);
            if (propertyInfo == null)
                throw new MissingFieldException($"This entity does not have such field: {relation}");

            var regexInsideSquareBrackets = new Regex(@"\[(.*?)\]");
            var propertyTypeFullPath = propertyInfo.ToString()!.Contains('[')
                ? regexInsideSquareBrackets.Match(propertyInfo.ToString()!).Groups[1].ToString()
                : propertyInfo.ToString()!.Split(' ')[0].Trim('{');

            var entryAssembly = type?.Assembly?.GetName()?.Name;
            if (entryAssembly == null) throw new Exception("Entry assembly is null");

            var newType = Type.GetType($"{propertyTypeFullPath}, {entryAssembly}");
            return TypeHasRelationProperty(newType, relation[(relation.IndexOf('.') + 1)..]);
        }

        if (type?.GetProperty(relation) == null)
            throw new MissingFieldException($"This entity does not have such field: {relation}");
        return true;
    }
    protected virtual IQueryable<TEntity> GetNoTrackingContextWithRelations(IEnumerable<string> relations)
    {
        var context = SetWithRelatedEntitiesAsNoTracking;
        context = relations.Aggregate(context, (current, relation) => current.Include(relation));
        return context;
    }

    public virtual IQueryable<TEntity> SearchQuery(Expression<Func<TEntity, bool>> predicate, string? sortBy = null,
       ListSortDirection? sortDirection = null,
       int? skip = null, int? take = null, List<string>? relations = null)
    {
        var context = relations != null && RelationsAreValid(relations)
            ? GetNoTrackingContextWithRelations(relations)
            : SetWithRelatedEntitiesAsNoTracking;

        if (string.IsNullOrEmpty(sortBy))
            return context.Where(predicate).TrySkip(skip ?? 0)
                .TryTake(take ?? int.MaxValue);

        //Checks if type has the sorting property ignoring case
        var propertyInfo = typeof(TEntity).GetProperties().SingleOrDefault(p => p.Name.ToLower() == sortBy.ToLower());

        if (propertyInfo == null)
            throw new ArgumentException(
                $"Entity {typeof(TEntity).FullName} doesn't have the property {sortBy} for sorting");

        //Ensure correct casing for property sorting
        sortBy = propertyInfo.Name;

        return context.Where(predicate)
            .OrderByDynamic(sortBy, (sortDirection ?? ListSortDirection.Ascending) == ListSortDirection.Descending)
            .TrySkip(skip)
            .TryTake(take);
    }
}
