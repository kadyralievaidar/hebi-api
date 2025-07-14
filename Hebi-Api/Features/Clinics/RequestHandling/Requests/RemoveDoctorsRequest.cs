using Hebi_Api.Features.Core.Common.RequestHandling;

namespace Hebi_Api.Features.Clinics.RequestHandling.Requests;

/// <summary>
///     Remove doctor from clinic
/// </summary>
public class RemoveDoctorsRequest : Request<Response>
{
    /// <summary>
    ///     List of doctor's id
    /// </summary>
    public List<Guid> DoctorIds { get; set; } = new List<Guid>();
    public RemoveDoctorsRequest(List<Guid> doctorIds)
    {
        DoctorIds = doctorIds;
    }
}
