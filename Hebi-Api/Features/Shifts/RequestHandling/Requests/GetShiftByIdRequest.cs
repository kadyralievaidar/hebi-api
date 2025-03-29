using Hebi_Api.Features.Core.Common.RequestHandling;

namespace Hebi_Api.Features.Shifts.RequestHandling.Requests;

public class GetShiftByIdRequest : Request<Response>
{
    public Guid ShiftId { get; set; }

    public GetShiftByIdRequest(Guid shiftId)
    {
        ShiftId = shiftId;
    }
}
