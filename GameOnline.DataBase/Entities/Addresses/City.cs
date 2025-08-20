namespace GameOnline.DataBase.Entities.Address;

public class City : BaseEntity
{
    public string CityName { get; set; }

    #region Relations
    public List<UserAddress> UserAddres { get; set; }
    #endregion

}