namespace Hebi_Api.Features.Core.DataAccess.Models;

public class Appointment : IBaseModel
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? FilePath { get; set; }

    /// <summary>
    ///     If it's new patient
    /// </summary>
    public string? PatientShortName { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid Id { get; set; }
    public DateTime? LastModifiedAt { get; set; }
    public bool IsDeleted { get; set; }

    #region Foreign Keys
    public Guid? LastModifiedBy { get; set; }
    public Guid? ClinicId { get; set; }
    public Guid? ShiftId { get; set; }
    public Shift? Shift { get; set; }
    public Guid? PatientId { get; set; }
    public ApplicationUser? Patient { get; set; }
    public Guid CreatedBy { get; set; }
    public ApplicationUser? Doctor { get; set; } = null!;
    public Guid? DoctorId { get; set; }

    #endregion
}
