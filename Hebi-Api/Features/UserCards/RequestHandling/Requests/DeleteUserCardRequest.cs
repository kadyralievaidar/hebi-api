using Hebi_Api.Features.Core.Common.RequestHandling;

namespace Hebi_Api.Features.UserCards.RequestHandling.Requests;

public class DeleteUserCardRequest : Request<Response>
{
    public Guid UserCardId { get; set; }

    public DeleteUserCardRequest(Guid userCardId)
    {
        UserCardId = userCardId;
    }
}
