using Hebi_Api.Features.Core.Common.RequestHandling;
using Hebi_Api.Features.Diseases.Dtos;

namespace Hebi_Api.Features.Diseases.RequestHandling.Requests;

/// <summary>
///     Update disease request
/// </summary>
public class UpdateDiseaseRequest : Request<Response>
{
    /// <summary>
    ///     Disease's id
    /// </summary>
    public Guid DiseaseId { get; set; }

    /// <summary>
    ///     CreateDiseaseDto
    /// </summary>
    public CreateDiseaseDto CreateDiseaseDto { get; set; }

    public UpdateDiseaseRequest(Guid diseaseId, CreateDiseaseDto createDiseaseDto)
    {
        DiseaseId = diseaseId;
        CreateDiseaseDto = createDiseaseDto;
    }
}
