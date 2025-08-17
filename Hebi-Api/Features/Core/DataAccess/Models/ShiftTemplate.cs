namespace Hebi_Api.Features.Core.DataAccess.Models;

/// <summary>
///     Shift template model
/// </summary>
public class ShiftTemplate : BaseModel, IBaseModel
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

    /// <summary>
    ///     Shift's duration
    /// </summary>
    public TimeSpan Duration
    {
        get
        {
            if (EndTime > StartTime)
                return EndTime - StartTime;

            // overnight shift
            return (TimeSpan.FromHours(24) - StartTime.ToTimeSpan()) + EndTime.ToTimeSpan();
        }
    }

    /// <summary>
    ///     Навигационное свойство для всех смен, созданных по этому шаблону
    /// </summary>
    public ICollection<Shift> Shifts { get; set; } = new List<Shift>();
}
