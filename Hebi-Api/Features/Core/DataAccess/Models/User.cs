using Microsoft.AspNetCore.Identity;

namespace Hebi_Api.Features.Core.DataAccess.Models;

public class User : IdentityUser<Guid>, IBaseModel
{
    public string? Patronymic { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public DateTime BirthDateTime { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? LastModifiedAt { get; set; }
    public bool IsDeleted { get; set; }

    /// <summary>
    ///     To control subscription
    /// </summary>
    public bool IsActive { get; set; }

    #region Foreign keys
    public Guid? LastModifiedBy { get; set; }
    public Guid CreatedBy { get; set; }
    public Guid? ClinicId { get; set; }
    public Clinic? Clinic { get; set; }
    #endregion
}
