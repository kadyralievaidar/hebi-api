using Hebi_Api.Features.Core.Common.RequestHandling;
using Hebi_Api.Features.Users.Dtos;

namespace Hebi_Api.Features.Users.RequestHandling.Requests;

/// <summary>
///     Change password request
/// </summary>
public class ChangePasswordRequest : Request<Response>
{
    /// <summary>
    ///     Change password dto
    /// </summary>
    public ChangePasswordDto Dto { get; set; }

    public ChangePasswordRequest(ChangePasswordDto dto)
    {
        Dto = dto;
    }
}
