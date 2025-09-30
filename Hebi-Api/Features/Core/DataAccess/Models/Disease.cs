namespace Hebi_Api.Features.Core.DataAccess.Models;

public class Disease : IBaseModel
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }

    /// <summary>
    /// Скидка в виде коэффициента: 0.9 = 10% скидка, 1 = без скидки
    /// </summary>
    private double _discount = 1;
    public double Discount
    {
        get => _discount;
        set => _discount = Math.Clamp(value, 0, 1); // чтобы всегда было 0..1
    }

    /// <summary>
    /// Цена с учётом скидки
    /// </summary>
    public decimal PriceWithDiscount => Math.Round(Price * (decimal)Discount, 2);

    public string? Color { get; set; } // Например HEX-код, "#FF0000"
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
