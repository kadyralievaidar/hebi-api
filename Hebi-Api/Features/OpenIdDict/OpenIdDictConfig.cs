using Hebi_Api.Features.Core.Common.Enums;
using Hebi_Api.Features.Core.DataAccess;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
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
                       .SetTokenEndpointUris("/connect/token")
                       .SetConfigurationEndpointUris("/.well-known/openid-configuration");

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
                       .EnableTokenEndpointPassthrough()
                       .DisableTransportSecurityRequirement();

                options.DisableAccessTokenEncryption(); // токен будет в виде обычного JWT

                options.AddEncryptionKey(new SymmetricSecurityKey(
                                     Convert.FromBase64String("DRjd/GnduI3Efzen9V9BvbNUfc/VKgXltV7Kbk9sMkY=")));
            }).AddValidation(options =>
            {
                options.SetIssuer(builder.Configuration.GetSection("OpenIdDict").GetValue<string>("Issuer"));
                options.AddEncryptionKey(new SymmetricSecurityKey(
                    Convert.FromBase64String("DRjd/GnduI3Efzen9V9BvbNUfc/VKgXltV7Kbk9sMkY=")));
                options.UseAspNetCore();
                options.UseSystemNetHttp();
            });
    }
    public static async Task SeedRoles(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

            var roles = new[] { UserRoles.SuperAdmin.ToString(), UserRoles.Admin.ToString(), UserRoles.Doctor.ToString(), 
                                UserRoles.Patient.ToString() };

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
                        Permissions.Scopes.Roles,
                        Scopes.OfflineAccess
                    }
                });
            }
        }
    }
}
