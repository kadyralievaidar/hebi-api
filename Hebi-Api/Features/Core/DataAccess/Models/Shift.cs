namespace Hebi_Api.Features.Core.DataAccess.Models;

public class Shift : IBaseModel
{
    public Guid Id { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? LastModifiedAt { get; set; }
    public bool IsDeleted { get; set; }

    #region Foreign Keys
    public Guid? ClinicId { get; set; }
    public Guid? LastModifiedBy { get; set; }
    public Guid CreatedBy { get; set; }
    public Guid DoctorId { get; set; }
    public User Doctor { get; set; } = null!;
    public IEnumerable<Appointment> Appointment { get; set;} = new List<Appointment>();
    #endregion
}
