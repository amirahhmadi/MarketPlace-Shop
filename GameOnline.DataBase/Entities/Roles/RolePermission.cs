using System.ComponentModel.DataAnnotations.Schema;

namespace GameOnline.DataBase.Entities.Role;

public class RolePermission : BaseEntity
{
    public int RoleId { get; set; }
    public int PermissionId{ get; set; }

    [ForeignKey(nameof(RoleId))]
    public Role Role { get; set; }
}