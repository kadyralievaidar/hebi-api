using Hebi_Api.Features.Core.Common;
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
        var sub = contextAccessor!.HttpContext!.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value!;
        return Guid.Parse(sub);
    }

    /// <summary>
    ///     Get user identifier
    /// </summary>
    /// <param name="contextAccessor">Http context accessor</param>
    /// <returns>User Id</returns>
    public static Guid GetClinicId(this IHttpContextAccessor contextAccessor)
    {
        var clinicId = contextAccessor!.HttpContext!.User.Claims.FirstOrDefault(x => x.Type == Consts.ClinicIdClaim)?.Value!;
        return Guid.Parse(clinicId);
    }
}
