using Hebi_Api.Features.Core.Common.RequestHandling;

namespace Hebi_Api.Features.Diseases.RequestHandling.Requests;

/// <summary>
///     Delete disease request
/// </summary>
public class DeleteDiseaseRequest : Request<Response>
{
    /// <summary>
    ///     Disease's id
    /// </summary>
    public Guid Id { get; set; }

    public DeleteDiseaseRequest(Guid id)
    {
        Id = id;
    }
}
