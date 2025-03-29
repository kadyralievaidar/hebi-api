using Hebi_Api.Features.Clinics.Dtos;
using Hebi_Api.Features.Core.Common.RequestHandling;

namespace Hebi_Api.Features.Clinics.RequestHandling.Requests;

public class UpdateClinicRequest : Request<Response>
{
    public Guid ClinicId { get; set; }
    public CreateClinicDto CreateClinicDto { get; set; }

    public UpdateClinicRequest(Guid clinicId, CreateClinicDto createClinicDto)
    {
        ClinicId = clinicId;
        CreateClinicDto = createClinicDto;
    }
}
