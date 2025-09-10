using Hebi_Api.Features.Core.Common.RequestHandling;
using Hebi_Api.Features.Users.Dtos;

namespace Hebi_Api.Features.Users.RequestHandling.Requests;

/// <summary>
///     Reset user's password
/// </summary>
public class ResetPasswordRequest : Request<Response>
{
    /// <summary>
    ///     Reset password dto
    /// </summary>
    public ResetPasswordDto Dto {  get; set; }

    public ResetPasswordRequest(ResetPasswordDto dto)
    {
        Dto = dto;
    }
}
