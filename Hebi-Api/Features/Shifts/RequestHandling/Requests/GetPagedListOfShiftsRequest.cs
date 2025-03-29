using Hebi_Api.Features.Core.Common.RequestHandling;
using Hebi_Api.Features.Shifts.Dtos;

namespace Hebi_Api.Features.Shifts.RequestHandling.Requests;

public class GetPagedListOfShiftsRequest : Request<Response>
{
    public GetPagedListOfShiftsDto Dto { get; set; }

    public GetPagedListOfShiftsRequest(GetPagedListOfShiftsDto dto)
    {
        Dto = dto;
    }
}
