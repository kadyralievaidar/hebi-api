using System.IdentityModel.Tokens.Jwt;
using Hebi_Api.Features.Core.Common.Enums;
using Hebi_Api.Features.Core.DataAccess;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
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
                       .EnableTokenEndpointPassthrough()
                       .DisableTransportSecurityRequirement();
                options.DisableAccessTokenEncryption(); // токен будет в виде обычного JWT
                options.AddDevelopmentEncryptionCertificate(); // временные ключи
                options.AddDevelopmentSigningCertificate();    // временные ключи
                options.AddEncryptionKey(new SymmetricSecurityKey(
                                     Convert.FromBase64String("DRjd/GnduI3Efzen9V9BvbNUfc/VKgXltV7Kbk9sMkY=")));

                // Register the signing credentials.
                options.AddDevelopmentSigningCertificate();
            })
            .AddValidation(options =>
            {
                // Note: the validation handler uses OpenID Connect discovery
                // to retrieve the issuer signing keys used to validate tokens.
                options.SetIssuer("https://localhost:7270/");

                // Register the encryption credentials. This sample uses a symmetric
                // encryption key that is shared between the server and the API project.
                //
                // Note: in a real world application, this encryption key should be
                // stored in a safe place (e.g in Azure KeyVault, stored as a secret).
                options.AddEncryptionKey(new SymmetricSecurityKey(
                    Convert.FromBase64String("DRjd/GnduI3Efzen9V9BvbNUfc/VKgXltV7Kbk9sMkY=")));

                // Register the System.Net.Http integration.
                options.UseSystemNetHttp();
                options.UseLocalServer();
                options.UseAspNetCore();
            });
        builder.Logging.AddFilter("OpenIddict", LogLevel.Debug);

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // <-- Add this
            options.Authority = "https://localhost:7270"; // your issuer
            options.Audience = "your_api_name"; // your API's name
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = "https://localhost:7270",
                ValidateAudience = true,
                ValidAudience = "your_api_name",
                ValidateLifetime = true,
                NameClaimType = "name",
                RoleClaimType = "role"
            };
        });
        builder.Services.AddAuthorization();
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
                        Permissions.Scopes.Roles,
                        Scopes.OfflineAccess
                    }
                });
            }
        }
    }
}
