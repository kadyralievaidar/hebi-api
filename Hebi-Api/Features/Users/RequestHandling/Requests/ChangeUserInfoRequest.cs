using Hebi_Api.Features.Core.Common.RequestHandling;
using Hebi_Api.Features.Users.Dtos;

namespace Hebi_Api.Features.Users.RequestHandling.Requests;

/// <summary>
///     Change user's info request
/// </summary>
public class ChangeUserInfoRequest : Request<Response>
{
    /// <summary>
    ///     User's basic info dto
    /// </summary>
    public BasicInfoDto Dto { get; set; }

    /// <summary>
    ///     ctor
    /// </summary>
    /// <param name="dto"></param>
    public ChangeUserInfoRequest(BasicInfoDto dto)
    {
        Dto = dto;
    }
}
