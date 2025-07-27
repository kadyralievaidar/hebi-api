using Hebi_Api.Features.Core.Common.RequestHandling;

namespace Hebi_Api.Features.Users.RequestHandling.Requests;

/// <summary>
///     Change user's role
/// </summary>
public class ChangeUserRoleRequest : Request<Response>
{
    /// <summary>
    ///     User's role
    /// </summary>
    public Guid? UserId { get; set; }

    /// <summary>
    ///     Role's name
    /// </summary>
    public string RoleName { get; set; }

    /// <summary>
    ///     Ctor
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="roleName"></param>
    public ChangeUserRoleRequest(Guid? userId, string roleName)
    {
        UserId = userId;
        RoleName = roleName;
    }
}
