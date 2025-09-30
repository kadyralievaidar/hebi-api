using System.Collections.ObjectModel;

namespace Hebi_Api.Features.Appointments.Dtos;

/// <summary>
///     Generate cycle appointments (i.e. brackets)
/// </summary>
public class GenerateAppointmentsDto
{
    /// <summary>
    ///     Start date of cycle
    /// </summary>
    public DateOnly StartDate { get; set; }

    /// <summary>
    ///     End date of cycle
    /// </summary>
    public DateOnly EndDate { get; set;}

    /// <summary>
    ///     Start time
    /// </summary>
    public TimeOnly StartTime { get; set; }

    /// <summary>
    ///     End time
    /// </summary>
    public TimeOnly EndTime { get; set; }
    /// <summary>
    ///     Collection of weekdays
    /// </summary>
    public ICollection<Guid> ShiftIds { get; set; } = new Collection<Guid>();

    /// <summary>
    ///     Sevice's id
    /// </summary>
    public Guid DiseaseId { get; set; }

    /// <summary>
    ///     User card's id
    /// </summary>
    public Guid? UserCardId { get; set; }
}
