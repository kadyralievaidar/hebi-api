using Hebi_Api.Features.Core.Common.RequestHandling;

namespace Hebi_Api.Features.Appointments.RequestHandling.Requests;

/// <summary>
///     Get appoitment by id request
/// </summary>
public class GetAppoitmentByIdRequest : Request<Response>
{
    /// <summary>
    ///     Appoinment id
    /// </summary>
    public Guid AppointmnetId { get; set; }
    public GetAppoitmentByIdRequest(Guid appointmnetId)
    {
        AppointmnetId = appointmnetId;
    }
}
