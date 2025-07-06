using Hebi_Api.Features.Clinics.Dtos;
using Hebi_Api.Features.Core.Common.RequestHandling;

namespace Hebi_Api.Features.Clinics.RequestHandling.Requests;

public class GetClinicWithDoctorsRequest: Request<Response>
{
    public GetClinicsDoctorsDto Dto { get; }

    public GetClinicWithDoctorsRequest(GetClinicsDoctorsDto dto)
    {
        Dto = dto;
    }
}
