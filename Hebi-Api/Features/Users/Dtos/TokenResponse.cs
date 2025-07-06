using System.Security.Claims;

namespace Hebi_Api.Features.Users.Dtos;

public class TokenResponse
{
    /// <summary>
    ///     ClaimsPrincipal
    /// </summary>
    public ClaimsPrincipal? Principal { get; set; }

    /// <summary>
    ///     Auth's scheme
    /// </summary>
    public string? AuthScheme { get; set; }
}
