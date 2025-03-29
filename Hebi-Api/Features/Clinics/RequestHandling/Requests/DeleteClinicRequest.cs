using Hebi_Api.Features.Core.Common.RequestHandling;

namespace Hebi_Api.Features.Clinics.RequestHandling.Requests;

public class DeleteClinicRequest : Request<Response>
{
    public Guid ClinicId { get; set; }

    public DeleteClinicRequest(Guid clinicId)
    {
        ClinicId = clinicId;
    }
}
