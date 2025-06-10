namespace Hebi_Api.Features.Core.DataAccess.Models;

public interface IBaseModel
{
    /// <summary>
    ///     Id of entity
    /// </summary>
    Guid Id { get; set; }
    /// <summary>
    ///     Last modified by
    /// </summary>
    Guid? LastModifiedBy { get; set; }

    /// <summary>
    ///     Created by
    /// </summary>
    Guid? CreatedBy { get; set; }
    /// <summary>
    ///     Created at datetime
    /// </summary>
    DateTime CreatedAt { get; set; }

    /// <summary>
    ///     Last modified at datetime
    /// </summary>
    DateTime? LastModifiedAt { get; set; }

    /// <summary>
    ///     Soft delete flag
    /// </summary>
    bool IsDeleted { get; set; }

    /// <summary>
    ///     Clinic Id filter
    /// </summary>
    Guid? ClinicId { get; set; }
}
