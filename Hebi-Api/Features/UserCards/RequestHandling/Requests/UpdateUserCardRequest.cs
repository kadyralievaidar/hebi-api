using Hebi_Api.Features.Core.Common.RequestHandling;
using Hebi_Api.Features.UserCards.Dtos;

namespace Hebi_Api.Features.UserCards.RequestHandling.Requests;

public class UpdateUserCardRequest : Request<Response>
{
    public Guid UserCardId { get; set; }

    public CreateUserCardDto Dto { get; set; }

    public UpdateUserCardRequest(Guid userCardId, CreateUserCardDto dto)
    {
        UserCardId = userCardId;
        Dto = dto;
    }
}
