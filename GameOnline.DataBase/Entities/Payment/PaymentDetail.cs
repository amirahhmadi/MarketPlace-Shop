using System.ComponentModel.DataAnnotations.Schema;
using GameOnline.DataBase.Entities.Carts;
using GameOnline.DataBase.Entities.Discounts;

namespace GameOnline.DataBase.Entities.Payment;

public class PaymentDetail : BaseEntity
{
    public int CartId { get; set; }
    public int Price { get; set; }
    public string RefId { get; set; }
    public string UserIp { get; set; }
    public string Authority { get; set; }
    public int DisCountId { get; set; }
    public byte PaymentType { get; set; }


    [ForeignKey(nameof(CartId))]
    public Cart Cart { get; set; }

    [ForeignKey(nameof(DisCountId))]
    public Discount Discount { get; set; }
}