using Hebi_Api.Features.Core.Common.RequestHandling;
using Hebi_Api.Features.Shifts.Dtos;

namespace Hebi_Api.Features.Shifts.RequestHandling.Requests;

public class UpdateShiftRequest : Request<Response>
{
    public Guid ShiftId { get; set; }
    public CreateShiftDto CreateShiftDto { get; set; }

    public UpdateShiftRequest(Guid shiftId, CreateShiftDto createShiftDto)
    {
        ShiftId = shiftId;
        CreateShiftDto = createShiftDto;
    }
}
