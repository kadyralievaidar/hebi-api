using Hebi_Api.Features.Core.Common;

namespace Hebi_Api.Features.OpenIdDict;

/// <summary>
///     Middleware to check clinic's claim
/// </summary>
public class ClaimsCheckMiddleware 
{
    private static List<PathString> pathStrings = new () 
    {
        new ("/Users/register"),
        new ("/connect/token"),
        new ("/Users/token"),
        new ("/Users/change-role"),
        new ("/Clinic/create-clinic")
    };
    /// <summary>
    ///     Request delegate to move to next
    /// </summary>
    private readonly RequestDelegate _next;

    /// <summary>
    ///     ctor
    /// </summary>
    /// <param name="next"></param>
    public ClaimsCheckMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    /// <summary>
    ///     Method to go to next middleware
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task InvokeAsync(HttpContext context)
    {
        // Ensure the user is authenticated
        var path = context.Request.Path;
        if (pathStrings.Contains(path))
        {
            await _next(context);
            return;
        }
        if (context.User?.Identity?.IsAuthenticated == true)
        {
            var clinicIdClaim = context.User.FindFirst(Consts.ClinicIdClaim)?.Value;

            if (!string.IsNullOrEmpty(clinicIdClaim))
            {
                await _next(context);
                return;
            }

            context.Response.StatusCode = Consts.ClinicNotFound;
            await context.Response.WriteAsync("Please change your role or create a clinic.");
            return;
        }

        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        await context.Response.WriteAsync("Unauthorized: Please log in.");
    }
}
