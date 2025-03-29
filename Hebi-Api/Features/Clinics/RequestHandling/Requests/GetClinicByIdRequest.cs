using Hebi_Api.Features.Core.Common.RequestHandling;

namespace Hebi_Api.Features.Clinics.RequestHandling.Requests;

public class GetClinicByIdRequest : Request<Response>
{
    public Guid ClinicId { get; set; }

    public GetClinicByIdRequest(Guid clinicId)
    {
        ClinicId = clinicId;
    }
}
