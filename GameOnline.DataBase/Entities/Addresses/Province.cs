namespace GameOnline.DataBase.Entities.Address;

public class Province : BaseEntity
{
    public string ProvinceName { get; set; }


    #region Relations
    public List<UserAddress> UserAddres { get; set; }
    #endregion
}