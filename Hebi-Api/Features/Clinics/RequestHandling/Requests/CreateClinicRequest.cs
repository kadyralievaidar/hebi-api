using Hebi_Api.Features.Clinics.Dtos;
using Hebi_Api.Features.Core.Common.RequestHandling;

namespace Hebi_Api.Features.Clinics.RequestHandling.Requests;

public class CreateClinicRequest : Request<Response>
{
    public CreateClinicDto CreateClinicDto { get; set; }

    public CreateClinicRequest(CreateClinicDto createClinicDto)
    {
        CreateClinicDto = createClinicDto;
    }
}
