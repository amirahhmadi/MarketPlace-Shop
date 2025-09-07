using System.ComponentModel.DataAnnotations.Schema;
using GameOnline.DataBase.Entities.Users;

namespace GameOnline.DataBase.Entities.Role;

public class UserRole : BaseEntity
{
    public int UserId { get; set; }
    public int RoleId { get; set; }

    [ForeignKey(nameof(UserId))]
    public User User { get; set; }

    [ForeignKey(nameof(RoleId))]
    public Role Role { get; set; }
}