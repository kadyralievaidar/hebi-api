using FluentValidation;
using Hebi_Api.Features.Appointments.Services;
using Hebi_Api.Features.Clinics.Services;
using Hebi_Api.Features.Core.Common.Validators;
using Hebi_Api.Features.Diseases.Services;
using Hebi_Api.Features.Shifts.Services;
using Hebi_Api.Features.UserCards.Services;
using Hebi_Api.Features.Users.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Hebi_Api.Features.Core.Extensions;

public static class StartupExtensions
{
    public static void RegisterRequestHandlers(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
        services.AddLocalization(options => options.ResourcesPath = "Resources");
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));
        services.AddSingleton(typeof(IValidator<>), typeof(CompositeValidator<>));
        services.AddTransient(typeof(IValidator<>), typeof(NoValidationValidator<>));

        services.Scan(scan => scan
            .FromCallingAssembly()
            .AddClasses(classes => classes.AssignableTo(typeof(IValidator<>)))
            .As(type => type.GetInterfaces()
                .Where(i => i.IsGenericType && !i.IsOpenGeneric() &&
                            i.GetGenericTypeDefinition() == typeof(IValidator<>)))
            .AsMatchingInterface()
            .WithTransientLifetime());
    }
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<IAppointmentsService, AppointmentsService>();
        services.AddScoped<IClinicsService, ClinicsService>();
        services.AddScoped<IDiseaseService, DiseasesService>();
        services.AddScoped<IShiftsService, ShiftsService>();
        services.AddScoped<IUserCardsService, UserCardsService>();
        services.AddScoped<IUsersService, UsersService>();
    }
}
