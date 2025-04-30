using Hebi_Api.Features.Core.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Hebi_Api.Features.Core.Common
{
    public static class DatabaseMigrator
    {
        public static async Task MigrateDatabaseAsync(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILogger<Program>>();
            var dbContext = services.GetRequiredService<HebiDbContext>();


            int retries = 5;
            while (retries > 0)
            {
                try
                {
                    logger.LogInformation("Applying migrations...");
                    await dbContext.Database.MigrateAsync();
                    logger.LogInformation("Migrations applied successfully");
                    break;
                }
                catch (Npgsql.PostgresException ex) when (ex.SqlState == "42P01")
                {
                    logger.LogWarning("Tables not found, applying migrations...");
                    await dbContext.Database.MigrateAsync();
                    break;
                }
                catch (Exception ex)
                {
                    retries--;
                    logger.LogWarning(ex, $"Migration failed, retries left: {retries}");

                    if (retries == 0)
                        throw;

                    await Task.Delay(5000);
                }
            }
        }
    }
}
