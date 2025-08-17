namespace Hebi_Api.Features.Core.DataAccess.Models;

public class Shift : BaseModel, IBaseModel 
{
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }

    #region Foreign Keys
    public Guid? DoctorId { get; set; }
    public ApplicationUser? Doctor { get; set; }
    public IEnumerable<Appointment> Appointments { get; set;} = new List<Appointment>();

    public Guid? ShiftTemplateId { get; set; }
    public ShiftTemplate? ShiftTemplate { get; set; }
    #endregion
}
