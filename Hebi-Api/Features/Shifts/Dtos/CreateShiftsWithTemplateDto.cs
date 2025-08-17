namespace Hebi_Api.Features.Shifts.Dtos;

/// <summary>
///     Create shift template with shift template id
/// </summary>
public class CreateShiftsWithTemplateDto
{
    /// <summary>
    ///     Shift template dto
    /// </summary>
    public Guid ShiftTemplateId { get; set; }

    /// <summary>
    ///     The start range of shifts
    /// </summary>
    public DateOnly StartDate {  get; set; }

    /// <summary>
    ///     The end range of shifts
    /// </summary>
    public DateOnly EndDate { get; set;}

    /// <summary>
    ///     Doctor's id
    /// </summary>
    public Guid? DoctorId { get; set; }

    /// <summary>
    ///     Days of weeks
    /// </summary>
    public List<DayOfWeek> DayOfWeeks { get; set; }
    public CreateShiftsWithTemplateDto(Guid shiftTemplateId, DateOnly startDate, DateOnly endDate, Guid? doctorId, List<DayOfWeek> dayOfWeeks)
    {
        ShiftTemplateId = shiftTemplateId;
        StartDate = startDate;
        EndDate = endDate;
        DoctorId = doctorId;
        DayOfWeeks = dayOfWeeks;
    }
}
