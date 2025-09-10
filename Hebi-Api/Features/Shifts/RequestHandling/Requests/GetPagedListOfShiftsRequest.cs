using Hebi_Api.Features.Core.Common.RequestHandling;
using Hebi_Api.Features.Shifts.Dtos;

namespace Hebi_Api.Features.Shifts.RequestHandling.Requests;

public class GetPagedListOfShiftsRequest : Request<Response>
{
    public GetListOfShiftsDto Dto { get; set; }

    public GetPagedListOfShiftsRequest(GetListOfShiftsDto dto)
    {
        Dto = dto;
    }
}
