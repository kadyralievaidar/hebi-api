namespace Hebi_Api.Features.Core.DataAccess.Models;

public class Disease : IBaseModel
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public double Discount { get; set; } = 1;
    public decimal PriceWithDiscount => Price * (decimal)Discount;
    public DateTime CreatedAt { get; set; }
    public DateTime? LastModifiedAt { get; set; }
    public bool IsDeleted { get; set; }

    #region Foreign keys
    public Guid? ClinicId { get; set; }
    public Guid? CreatedBy { get; set; }
    public Guid? LastModifiedBy { get; set; }
    public Guid? Appointment { get; set; }

    #endregion
}
