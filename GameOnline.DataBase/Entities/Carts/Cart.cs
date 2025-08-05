using System.ComponentModel.DataAnnotations.Schema;
using GameOnline.DataBase.Entities.Payment;
using GameOnline.DataBase.Entities.Users;

namespace GameOnline.DataBase.Entities.Carts;

public class Cart : BaseEntity
{
    public string Province { get; set; }
    public string City { get; set; }
    public string FullAddress { get; set; }
    public int UserId { get; set; }
    public byte OrderType { get; set; }

    [ForeignKey(nameof(UserId))]
    public User User { get; set; }

    public List<CartDetail> CartDetails { get; set; }
    public List<PaymentDetail> PaymentDetails { get; set; }
}