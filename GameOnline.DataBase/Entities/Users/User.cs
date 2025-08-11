using GameOnline.DataBase.Entities.Carts;
using GameOnline.DataBase.Entities.Comment_FAQ;

namespace GameOnline.DataBase.Entities.Users;

public class User : BaseEntity
{
    public string Email { get; set; }
    public string? Phone { get; set; }
    public string? ImgName { get; set; }
    public string? Name { get; set; }
    public string Password { get; set; }
    public string? CartNumber { get; set; }
    public string? NationalCode { get; set; }
    public byte type { get; set; }
    public string? ActiveCode { get; set; }

    public List<Cart> Carts { get; set; }
    public List<Question> Questions { get; set; }
    public List<FAQAnswer> FaqAnswers { get; set; }
}