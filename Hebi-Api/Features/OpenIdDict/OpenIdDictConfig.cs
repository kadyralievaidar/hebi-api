using Hebi_Api.Features.Core.Common;
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
                       .SetRevocationEndpointUris("/connect/revocation")
                       .SetConfigurationEndpointUris("/.well-known/openid-configuration");

                options.AddDevelopmentEncryptionCertificate()
                       .AddDevelopmentSigningCertificate();

                // Настройка токенов
                options.AddEphemeralEncryptionKey()
                       .AddEphemeralSigningKey();

                // Настройка времени жизни токенов
                options.SetAccessTokenLifetime(TimeSpan.FromMinutes(15));
                options.SetRefreshTokenLifetime(TimeSpan.FromHours(8));

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
                options.UseLocalServer();
                options.SetIssuer(builder.Configuration.GetSection("OpenIdDict").GetValue<string>("Issuer"));
                options.AddEncryptionKey(new SymmetricSecurityKey(
                    Convert.FromBase64String("DRjd/GnduI3Efzen9V9BvbNUfc/VKgXltV7Kbk9sMkY=")));
                options.UseAspNetCore();
                options.UseSystemNetHttp();
                options.Configure(options =>
                {
                    options.TokenValidationParameters.NameClaimType = Consts.UserId;
                    options.TokenValidationParameters.RoleClaimType = Consts.Role;
                });
            });
    }
    public static async Task SeedRoles(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

            var roles = new[] { Consts.SuperAdmin, Consts.Individual,
                                Consts.Admin, Consts.Doctor, 
                                Consts.Patient };

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

            var clientId = "hebi_client";
            var existingApp = await manager.FindByClientIdAsync(clientId);

            var requiredPermissions = new[]
            {
                Permissions.Endpoints.Token,
                Permissions.GrantTypes.Password,
                Permissions.GrantTypes.RefreshToken,
                Permissions.Endpoints.Revocation,
                Scopes.OpenId,
                Permissions.Scopes.Profile,
                Permissions.Scopes.Email,
                Permissions.Scopes.Roles,
                Scopes.OfflineAccess
            };

            if (existingApp is null)
            {
                var descriptor = new OpenIddictApplicationDescriptor
                {
                    ClientId = clientId,
                    ClientType = ClientTypes.Public
                };

                foreach (var permission in requiredPermissions)
                {
                    descriptor.Permissions.Add(permission);
                }

                await manager.CreateAsync(descriptor);
            }
            else
            {
                var descriptor = new OpenIddictApplicationDescriptor();
                await manager.PopulateAsync(existingApp, descriptor);

                // Ensure critical fields are not lost
                descriptor.ClientId ??= clientId;
                descriptor.ClientType ??= ClientTypes.Public;

                // Merge current + required permissions to avoid losing anything
                var mergedPermissions = new HashSet<string>(descriptor.Permissions);
                mergedPermissions.UnionWith(requiredPermissions);

                // Clear and re-add to avoid duplicates or missing items
                descriptor.Permissions.Clear();
                foreach (var permission in mergedPermissions)
                {
                    descriptor.Permissions.Add(permission);
                }

                await manager.UpdateAsync(existingApp, descriptor);
            }
        }
    }
}
