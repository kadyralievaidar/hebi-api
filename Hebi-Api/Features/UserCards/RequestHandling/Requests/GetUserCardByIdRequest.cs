using Hebi_Api.Features.Core.Common.RequestHandling;

namespace Hebi_Api.Features.UserCards.RequestHandling.Requests;

public class GetUserCardByIdRequest : Request<Response>
{
    public Guid UserCardId { get; set; }

    public GetUserCardByIdRequest(Guid userCardId)
    {
        UserCardId = userCardId;
    }
}
