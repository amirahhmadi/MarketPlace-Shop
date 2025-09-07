namespace GameOnline.DataBase.Entities.Role;

public class Role : BaseEntity
{
    public string RoleTitle { get; set; }

    public List<RolePermission> RolePermissions { get; set; }
    public List<UserRole> UserRoles { get; set; }
}