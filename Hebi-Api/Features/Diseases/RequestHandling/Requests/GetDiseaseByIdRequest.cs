using Hebi_Api.Features.Core.Common.RequestHandling;

namespace Hebi_Api.Features.Diseases.RequestHandling.Requests;

/// <summary>
///     GetDiseaseByIdRequest
/// </summary>
public class GetDiseaseByIdRequest : Request<Response>
{
    /// <summary>
    ///     Disease's id
    /// </summary>
    public Guid DiseaseId { get; set; }

    public GetDiseaseByIdRequest(Guid diseaseId)
    {
        DiseaseId = diseaseId;
    }
}
