using System.ComponentModel.DataAnnotations.Schema;
using GameOnline.DataBase.Entities.Products;
using GameOnline.DataBase.Entities.Users;

namespace GameOnline.DataBase.Entities.Comment_FAQ;

public class Question :BaseEntity
{
    public int UserId { get; set; }
    public int ProductId { get; set; }
    public string QuestionText { get; set; }
    public bool IsConfirm { get; set; }

    [ForeignKey(nameof(UserId))]
    public User User { get; set; }

    [ForeignKey(nameof(ProductId))] 
    public Product Product { get; set; }

    public List<FAQAnswer> FaqAnswers { get; set; }

}