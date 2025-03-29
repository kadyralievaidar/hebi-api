using Hebi_Api.Features.Clinics.Dtos;
using Hebi_Api.Features.Core.Common.RequestHandling;

namespace Hebi_Api.Features.Clinics.RequestHandling.Requests;

/// <summary>
///     Get paged list clinic request
/// </summary>
public class GetPagedListClinicRequest : Request<Response>
{
    public GetPagedListOfClinicDto Dto { get; set; }

    public GetPagedListClinicRequest(GetPagedListOfClinicDto dto)
    {
        Dto = dto;
    }
}
