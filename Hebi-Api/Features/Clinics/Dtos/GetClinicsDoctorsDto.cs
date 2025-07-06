using Hebi_Api.Features.Core.Common.RequestHandling;

namespace Hebi_Api.Features.Clinics.Dtos;

public class GetClinicsDoctorsDto : BaseListRequest
{
    public Guid ClinicId { get; set; }
}
