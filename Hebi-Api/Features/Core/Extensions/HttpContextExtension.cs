using System.Security.Claims;

namespace Hebi_Api.Features.Core.Extensions;

public static class HttpContextExtension
{
    /// <summary>
    ///     Get user identifier
    /// </summary>
    /// <param name="contextAccessor">Http context accessor</param>
    /// <returns>User Id</returns>
    public static Guid GetUserIdentifier(this IHttpContextAccessor contextAccessor)
    {
        var sub = contextAccessor!.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
        return Guid.Parse(sub);
    }
}
