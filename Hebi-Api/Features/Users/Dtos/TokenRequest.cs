using MediatR;
using OpenIddict.Abstractions;

namespace Hebi_Api.Features.Users.Dtos;

public class TokenRequest : IRequest<TokenResponse>
{
    /// <summary>
    ///     Open id dict request
    /// </summary>
    public OpenIddictRequest Request { get; set; }

    public TokenRequest(OpenIddictRequest request)
    {
        Request = request;
    }
}
