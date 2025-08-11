using System.ComponentModel.DataAnnotations.Schema;
using GameOnline.DataBase.Entities.Users;

namespace GameOnline.DataBase.Entities.Comment_FAQ;

public class FAQAnswer : BaseEntity
{
    public int UserId { get; set; }
    public int QuestionId { get; set; }
    public string AnswerText { get; set; }
    public bool IsConfirm { get; set; }

    [ForeignKey(nameof(UserId))]
    public User User { get; set; }

    [ForeignKey(nameof(QuestionId))]
    public Question Question { get; set; }
}