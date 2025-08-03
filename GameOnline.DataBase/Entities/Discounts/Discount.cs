namespace GameOnline.DataBase.Entities.Discounts;

public class Discount : BaseEntity
{
    public int Code { get; set; }
    public int? UserCount { get; set; }
    public bool IsActive { get; set; }
    public DateTime? StartDiscount { get; set; }
    public DateTime? EndDiscount { get; set; }
}