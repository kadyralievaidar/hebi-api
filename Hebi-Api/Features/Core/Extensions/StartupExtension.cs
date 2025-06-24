using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using FluentValidation;
using Hebi_Api.Features.Appointments.Services;
using Hebi_Api.Features.Clinics.Services;
using Hebi_Api.Features.Core.Common.Validators;
using Hebi_Api.Features.Diseases.Services;
using Hebi_Api.Features.Shifts.Services;
using Hebi_Api.Features.UserCards.Services;
using Hebi_Api.Features.Users.Dtos;
using Hebi_Api.Features.Users.RequestHandling.Validators;
using Hebi_Api.Features.Users.Services;
using MediatR;
using Microsoft.OpenApi.Models;

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
        //services.AddScoped<IValidator<TokenRequest>, TokenRequestValidator>();
    }

    public static void AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "You api title", Version = "v1" });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement()
             {
               {
                 new OpenApiSecurityScheme
                 {
                   Reference = new OpenApiReference
                     {
                       Type = ReferenceType.SecurityScheme,
                       Id = "Bearer"
                     },
                     Scheme = "oauth2",
                     Name = "Bearer",
                     In = ParameterLocation.Header,

                   },
                   new List<string>()
                 }
               });
               }
             );
    }
}
