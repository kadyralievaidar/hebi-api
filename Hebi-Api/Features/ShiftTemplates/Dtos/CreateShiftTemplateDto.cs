namespace Hebi_Api.Features.ShiftTemplates.Dtos;

public class CreateShiftTemplateDto
{
    /// <summary>
    ///     Shift template name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    ///     Shift template start time (Time only)
    /// </summary>
    public TimeOnly StartTime { get; set; }

    /// <summary>
    ///     Shift template end time (Time only)
    /// </summary>
    public TimeOnly EndTime { get; set; }
}
