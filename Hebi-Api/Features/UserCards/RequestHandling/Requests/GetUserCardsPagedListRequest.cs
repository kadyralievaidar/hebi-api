using Hebi_Api.Features.Core.Common.RequestHandling;
using Hebi_Api.Features.UserCards.Dtos;

namespace Hebi_Api.Features.UserCards.RequestHandling.Requests;

public class GetUserCardsPagedListRequest : Request<Response>
{
    public GetPagedListOfUserCardDto Dto { get; set; }

    public GetUserCardsPagedListRequest(GetPagedListOfUserCardDto dto)
    {
        Dto = dto;
    }
}
