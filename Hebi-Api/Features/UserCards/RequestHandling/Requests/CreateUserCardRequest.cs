using Hebi_Api.Features.Core.Common.RequestHandling;
using Hebi_Api.Features.UserCards.Dtos;

namespace Hebi_Api.Features.UserCards.RequestHandling.Requests;

public class CreateUserCardRequest : Request<Response>
{
    public CreateUserCardDto CreateUserCardDto { get; set; }

    public CreateUserCardRequest(CreateUserCardDto createUserCardDto)
    {
        CreateUserCardDto = createUserCardDto;
    }
}
