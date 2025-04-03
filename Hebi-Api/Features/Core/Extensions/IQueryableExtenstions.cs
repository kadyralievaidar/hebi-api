using Hebi_Api.Features.Core.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace Hebi_Api.Features.Core.Extensions;

public static class QueryableExtensions
{
    /// <summary>
    ///     Trying to skip query
    /// </summary>
    /// <param name="query"></param>
    /// <param name="skip"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IQueryable<T> TrySkip<T>(this IQueryable<T> query, int? skip)
    {
        return skip.HasValue ? query.Skip(skip.Value).AsQueryable() : query;
    }

    /// <summary>
    ///     Trying to take query
    /// </summary>
    /// <param name="query"></param>
    /// <param name="take"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IQueryable<T> TryTake<T>(this IQueryable<T> query, int? take)
    {
        return take.HasValue ? query.Take(take.Value).AsQueryable() : query;
    }

    /// <summary>
    /// Dynamically creates a call like: query.OrderBy(p => p.SortColumn)
    /// </summary>
    /// <param name="query"></param>
    /// <param name="sortColumn"></param>
    /// <param name="descending"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static IQueryable<T> OrderByDynamic<T>(
        this IQueryable<T> query,
        string sortColumn,
        bool descending)
    {
        var parameter = Expression.Parameter(typeof(T), "p");
        var propertyExpression = GetPropertyExpression(parameter, sortColumn);

        var lambdaExpression = Expression.Lambda(propertyExpression, parameter);

        var methodName = descending ? "OrderByDescending" : "OrderBy";
        var methodCallExpression = Expression.Call(
            typeof(Queryable),
            methodName,
            new[] { typeof(T), propertyExpression.Type },
            query.Expression,
            Expression.Quote(lambdaExpression));

        return query.Provider.CreateQuery<T>(methodCallExpression);
    }

    /// <summary>
    /// Builds the property access expression to p.SortColumn
    /// </summary>
    private static Expression GetPropertyExpression(Expression parameter, string propertyPath)
    {
        var properties = propertyPath.Split('.');
        var propertyExpression = parameter;
        foreach (var property in properties)
        {
            var propertyInfo = propertyExpression.Type.GetProperty(property,
                BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (propertyInfo == null)
            {
                throw new ArgumentException($"Property {property} not found on {propertyExpression.Type.FullName}");
            }

            propertyExpression = Expression.MakeMemberAccess(propertyExpression, propertyInfo);
        }

        return propertyExpression;
    }

    /// <summary>
    ///     Search paged with dynamic parameters
    /// </summary>
    /// <param name="context"></param>
    /// <param name="predicate"></param>
    /// <param name="sortBy"></param>
    /// <param name="sortDirection"></param>
    /// <param name="skip"></param>
    /// <param name="take"></param>
    /// <returns></returns>
    public static IQueryable<T> SearchQuery<T>(
        this IQueryable<T> context,
        Expression<Func<T, bool>> predicate,
        string? sortBy = null,
        ListSortDirection? sortDirection = null,
        int? skip = null,
        int? take = null)
    {
        if (string.IsNullOrEmpty(sortBy))
            return context.Where(predicate).TrySkip(skip ?? 0)
                .TryTake(take ?? int.MaxValue);

        //Checks if type has the sorting property ignoring case
        var propertyInfo = typeof(T).GetProperties().SingleOrDefault(p => p.Name.ToLower() == sortBy.ToLower());

        if (propertyInfo == null)
            throw new ArgumentException(
                $"Entity {typeof(T).FullName} doesn't have the property {sortBy} for sorting");

        //Ensure correct casing for property sorting
        sortBy = propertyInfo.Name;

        return context.Where(predicate)
            .OrderByDynamic(sortBy, (sortDirection ?? ListSortDirection.Ascending) == ListSortDirection.Descending).TrySkip(skip)
            .TryTake(take);
    }

    /// <summary>
    ///     Continue including related entities.
    /// </summary>
    /// <typeparam name="T">Key entity</typeparam>
    /// <typeparam name="TProperty">Related entity</typeparam>
    /// <param name="query">Db query</param>
    /// <param name="navigationPropertyPath">Property path</param>
    /// <returns>Extended query</returns>
    public static IIncludableQueryable<T, TProperty> ContinueIncluding<T, TProperty>(this IQueryable<T> query,
        Expression<Func<T, TProperty>> navigationPropertyPath)
        where T : class, IBaseModel
        where TProperty : class
    {
        return query.Include(navigationPropertyPath);
    }

    /// <summary>
    ///     Continue including chain of related entities.
    /// </summary>
    /// <typeparam name="T">Key entity</typeparam>
    /// <typeparam name="TPreviousProperty">Previous entity</typeparam>
    /// <typeparam name="TProperty">Related entity</typeparam>
    /// <param name="query">Db query</param>
    /// <param name="navigationPropertyPath">Property path</param>
    /// <returns>Extended query</returns>
    public static IIncludableQueryable<T, TProperty> ThenIncluding<T, TPreviousProperty, TProperty>(
        this IIncludableQueryable<T, IEnumerable<TPreviousProperty>> query,
        Expression<Func<TPreviousProperty, TProperty>> navigationPropertyPath)
        where T : class, IBaseModel
        where TPreviousProperty : class, IBaseModel
        where TProperty : class, IBaseModel
    {
        return query.ThenInclude(navigationPropertyPath);
    }
}
