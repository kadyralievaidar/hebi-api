namespace Hebi_Api.Features.Core.DataAccess.Models;

public class UserCard : IBaseModel
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? LastModifiedAt { get; set; }
    public bool IsDeleted { get; set; }

    #region Foreign keys
    public Guid? ClinicId { get; set; }
    public Guid? LastModifiedBy { get; set; }
    public Guid? CreatedBy { get; set; }
    public IEnumerable<Appointment>? Appointment { get; set; } = new List<Appointment>();
    public Guid PatientId { get; set; } 
    public ApplicationUser? Patient { get; set; } = null!;
    #endregion
}
