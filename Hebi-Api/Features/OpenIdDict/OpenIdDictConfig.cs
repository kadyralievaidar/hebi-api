using Hebi_Api.Features.Core.Common.Enums;
using Hebi_Api.Features.Core.DataAccess;
using Microsoft.AspNetCore.Identity;
using OpenIddict.Abstractions;
using OpenIddict.Validation.AspNetCore;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Hebi_Api.Features.OpenIdDict;

public static class OpenIdDictConfig
{
    public static void AddOpenIdDict(this WebApplicationBuilder builder)
    {
        builder.Services.AddOpenIddict()
            .AddCore(options =>
            {
                options.UseEntityFrameworkCore()
                       .UseDbContext<HebiDbContext>();
            })
            .AddServer(options =>
            {
                options.AllowAuthorizationCodeFlow()
                       .AllowPasswordFlow()
                       .RequireProofKeyForCodeExchange()
                       .AllowRefreshTokenFlow();

                options.SetAuthorizationEndpointUris("/connect/authorize")
                       .SetTokenEndpointUris("/connect/token");

                options.AddDevelopmentEncryptionCertificate()
                       .AddDevelopmentSigningCertificate();

                // Настройка токенов
                options.AddEphemeralEncryptionKey()
                       .AddEphemeralSigningKey();

                // Настройка времени жизни токенов
                options.SetAccessTokenLifetime(TimeSpan.FromHours(1));
                options.SetRefreshTokenLifetime(TimeSpan.FromDays(7));

                options.RegisterScopes(Scopes.Email,
                      Scopes.Profile,
                      Scopes.Roles,
                      Scopes.OfflineAccess,
                      "api");

                options.UseAspNetCore()
                       .EnableAuthorizationEndpointPassthrough()
                       .EnableTokenEndpointPassthrough();
            })
            .AddValidation(options =>
            {
                options.UseLocalServer();
                options.UseAspNetCore();
            });
        builder.Logging.AddFilter("OpenIddict", LogLevel.Debug);

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
        });
    }
    public static async Task SeedRoles(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

            var roles = new[] { UserRoles.SuperAdmin.ToString(), UserRoles.Admin.ToString(), UserRoles.Doctor.ToString(), UserRoles.Patient.ToString() };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole<Guid>(role));
                }
            }
        }
        using (var scope = app.Services.CreateScope())
        {
            var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();

            if (await manager.FindByClientIdAsync("hebi_client") is null)
            {
                await manager.CreateAsync(new OpenIddictApplicationDescriptor
                {
                    ClientId = "hebi_client",
                    ClientType = ClientTypes.Public,
                    Permissions =
                    {
                        Permissions.Endpoints.Token,
                        Permissions.GrantTypes.Password,
                        Permissions.GrantTypes.RefreshToken,
                        Scopes.OpenId,
                        Permissions.Scopes.Profile,
                        Permissions.Scopes.Email,
                        Permissions.Scopes.Roles
                    }
                });
            }
        }
    }
}
