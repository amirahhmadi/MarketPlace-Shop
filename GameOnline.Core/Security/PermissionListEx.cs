namespace GameOnline.Core.Security
{
    public class PermissionListEx
    {
        public string PermissionTitle { get; set; }
        public int PermissionId { get; set; }

        public List<PermissionListEx> permissionList()
        {
            List<PermissionListEx> list = new List<PermissionListEx>()
            {
                new PermissionListEx() { PermissionTitle ="مدیر", PermissionId = 1 },
                new PermissionListEx() { PermissionTitle ="مدیرگالری", PermissionId = 2 },
                new PermissionListEx() { PermissionTitle ="مدیر گارانتی", PermissionId = 3 },
                new PermissionListEx() { PermissionTitle ="مدیر برند", PermissionId = 4 },
            };

            return list;
        }

    }
}
