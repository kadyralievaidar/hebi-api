using Hebi_Api.Features.Core.Common.RequestHandling;
using Hebi_Api.Features.Users.Dtos;

namespace Hebi_Api.Features.Users.RequestHandling.Requests;

/// <summary>
///     Create patient 
/// </summary>
public class CreatePatientRequest : Request<Response>
{
    /// <summary>
    ///     Create patient dto
    /// </summary>
    public CreatePatientDto Dto { get; set; }

    public CreatePatientRequest(CreatePatientDto dto)
    {
        Dto = dto;
    }
}
