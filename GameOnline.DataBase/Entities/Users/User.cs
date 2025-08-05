using GameOnline.DataBase.Entities.Carts;

namespace GameOnline.DataBase.Entities.Users;

public class User : BaseEntity
{
    public string Email { get; set; }
    public string? Phone { get; set; }
    public string? ImageName { get; set; }
    public string? Name { get; set; }

    public List<Cart> Carts { get; set; }
}