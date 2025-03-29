using Hebi_Api.Features.Core.Common.RequestHandling;
using Hebi_Api.Features.Diseases.Dtos;

namespace Hebi_Api.Features.Diseases.RequestHandling.Requests;

/// <summary>
///     CreateDiseaseRequest
/// </summary>
public class CreateDiseaseRequest : Request<Response>
{
    /// <summary>
    ///     Dto
    /// </summary>
    public CreateDiseaseDto CreateDiseaseDto { get; set; }

    public CreateDiseaseRequest(CreateDiseaseDto createDiseaseDto)
    {
        CreateDiseaseDto = createDiseaseDto;
    }
}
