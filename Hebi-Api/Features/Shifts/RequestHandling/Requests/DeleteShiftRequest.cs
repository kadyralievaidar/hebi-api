using Hebi_Api.Features.Core.Common.RequestHandling;

namespace Hebi_Api.Features.Shifts.RequestHandling.Requests;

public class DeleteShiftRequest : Request<Response>
{
    public Guid ShiftId {  get; set; }
    public DeleteShiftRequest(Guid shiftId)
    {
        ShiftId = shiftId;
    }
}
