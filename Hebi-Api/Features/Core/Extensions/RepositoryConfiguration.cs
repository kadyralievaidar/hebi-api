using Hebi_Api.Features.Core.DataAccess.Models;
using Hebi_Api.Features.Core.DataAccess.Repositories;
using Hebi_Api.Features.Core.DataAccess.UOW;
using Hebi_Api.Features.Core.DataAccess;
using System.Reflection;
using Hebi_Api.Features.Core.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Hebi_Api.Features.Core.Extensions;

/// <summary>
///     Configuration of repository
/// </summary>
public static class RepositoryConfiguration
{
    /// <summary>
    ///     Configure repository
    /// </summary>
    /// <param name="builder">Web application builder</param>
    public static void ConfigureRepository(this WebApplicationBuilder builder)
    {
        builder.AddDbContext();
    }

    /// <summary>
    ///     Create scope for Database
    /// </summary>
    /// <param name="builder">Services сollection</param>
    private static void AddDbContext(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<HebiDbContext>(options =>
        {
            options.EnableSensitiveDataLogging();

            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"), b =>
            {
                b.MigrationsAssembly(typeof(HebiDbContext).Assembly.FullName);
                b.MigrationsHistoryTable(HistoryRepository.DefaultTableName);
            });
        });
        builder.Services.AddScoped<IDbTransaction, DbTransaction>();
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        builder.Services.RegisterGenericRepository();
        builder.Services.RegisterCustomRepository();
    }

    /// <summary>
    ///     Register custom repository
    /// </summary>
    /// <param name="services">Service collection</param>
    private static void RegisterCustomRepository(this IServiceCollection services)
    {
        services.AddScoped<IUsersRepository, UsersRepository>();
        services.AddScoped<IDiseasesRepository, DiseasesRepository>();
        services.AddScoped<IAppointmentsRepository, AppointmentsRepository>();
        services.AddScoped<IUserCardsRepository, UserCardsRepository>();
        services.AddScoped<IClinicsRepository, ClinicsRepository>();
        services.AddScoped<IShiftsRepository, ShiftsRepository>();
        services.AddScoped<IUserCardsRepository, UserCardsRepository>();
    }

    /// <summary>
    ///     Register generic repository
    /// </summary>
    /// <param name="services">Service collection</param>
    private static void RegisterGenericRepository(this IServiceCollection services) =>
        (from type in Assembly.GetAssembly(typeof(Appointment))?.GetTypes()
         from interfaceType in type.GetInterfaces()
         where !type.IsAbstract && typeof(IBaseModel).IsAssignableFrom(interfaceType)
         select type).Distinct().ToList().ForEach(genericRepositoryType =>
         {
             var serviceType = typeof(IGenericRepository<>).MakeGenericType(genericRepositoryType);
             var implementationType = typeof(GenericRepository<>).MakeGenericType(genericRepositoryType);
             services.AddScoped(serviceType, implementationType);
         });
}