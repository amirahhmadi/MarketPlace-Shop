using GameOnline.DataBase.Entities.Payment;

namespace GameOnline.DataBase.Entities.Discounts;

public class Discount : BaseEntity
{
    public string Code { get; set; }
    public int? UserCount { get; set; }
    public bool IsActive { get; set; }
    public DateTime? StartDiscount { get; set; }
    public DateTime? EndDiscount { get; set; }


    public List<PaymentDetail> PaymentDetails { get; set; }
}