
namespace Hebi_Api.Features.Core.DataAccess.Models;

public class BaseModel : IBaseModel
{
    public Guid Id { get; set ; }
    public Guid? LastModifiedBy { get ; set ; }
    public Guid CreatedBy { get ; set ; }
    public DateTime CreatedAt { get ; set ; }
    public DateTime? LastModifiedAt { get ; set ; }
    public bool IsDeleted { get ; set ; }
    public Guid? ClinicId { get ; set ; }
}
