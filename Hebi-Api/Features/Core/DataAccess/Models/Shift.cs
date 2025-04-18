namespace Hebi_Api.Features.Core.DataAccess.Models;

public class Shift : BaseModel, IBaseModel 
{
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }

    #region Foreign Keys
    public Guid DoctorId { get; set; }
    public ApplicationUser Doctor { get; set; } = null!;
    public IEnumerable<Appointment> Appointment { get; set;} = new List<Appointment>();
    #endregion
}
