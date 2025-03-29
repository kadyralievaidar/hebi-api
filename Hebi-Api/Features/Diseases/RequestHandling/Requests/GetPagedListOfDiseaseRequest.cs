using Hebi_Api.Features.Core.Common.RequestHandling;
using Hebi_Api.Features.Diseases.Dtos;

namespace Hebi_Api.Features.Diseases.RequestHandling.Requests;

/// <summary>
///     GetPagedListOfDiseaseRequest
/// </summary>
public class GetPagedListOfDiseaseRequest : Request<Response>
{
    /// <summary>
    ///     GetPagedListOfDiseaseDto
    /// </summary>
    public GetPagedListOfDiseaseDto Dto { get; set; }

    public GetPagedListOfDiseaseRequest(GetPagedListOfDiseaseDto dto)
    {
        Dto = dto;
    }
}
