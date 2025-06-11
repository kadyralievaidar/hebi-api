using Microsoft.AspNetCore.Identity;

namespace Hebi_Api.Features.Core.DataAccess.Models;

public class ApplicationUser : IdentityUser<Guid>, IBaseModel
{
    public string? Patronymic { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime? BirthDateTime { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastModifiedAt { get; set; }
    public bool IsDeleted { get; set; } = false;

    /// <summary>
    ///     To control subscription
    /// </summary>
    public bool IsActive { get; set; } = true;

    #region Foreign keys
    public Guid? LastModifiedBy { get; set; }
    public Guid? CreatedBy { get; set; } = Guid.Empty;
    public Guid? ClinicId { get; set; }
    public Clinic? Clinic { get; set; }
    #endregion
}
