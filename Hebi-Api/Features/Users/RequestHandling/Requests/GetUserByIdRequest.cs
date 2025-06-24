using Hebi_Api.Features.Core.Common.RequestHandling;

namespace Hebi_Api.Features.Users.RequestHandling.Requests;

/// <summary>
///     Get user by id request
/// </summary>
public class GetUserByIdRequest : Request<Response>
{
    /// <summary>
    ///     User's id
    /// </summary>
    public Guid UserId { get; set; }

    public GetUserByIdRequest(Guid userId)
    {
        UserId = userId;
    }
}
