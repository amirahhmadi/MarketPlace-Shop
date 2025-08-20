using GameOnline.DataBase.Entities.Users;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameOnline.DataBase.Entities.Address;

public class UserAddress : BaseEntity
{
    public int UserId { get; set; }
    public int ProvinceId { get; set; }
    public int CityId { get; set; }
    public string? FullAddress { get; set; }
    public bool IsActive { get; set; }
    public string Phone { get; set; }
    public string PostalCode { get; set; }
    public string UserName { get; set; }

    #region Relations
    [ForeignKey(nameof(UserId))]
    public User User { get; set; }

    [ForeignKey(nameof(ProvinceId))]
    public Province Province { get; set; }

    [ForeignKey(nameof(CityId))]
    public City City { get; set; }
    #endregion
}