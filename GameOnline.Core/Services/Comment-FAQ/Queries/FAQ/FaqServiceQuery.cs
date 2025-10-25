using GameOnline.Core.ExtenstionMethods;
using GameOnline.Core.ViewModels.Comment_FAQ.FAQ.Client;
using GameOnline.DataBase.Context;
using Microsoft.EntityFrameworkCore;

namespace GameOnline.Core.Services.Comment_FAQ.Queries.FAQ;

public class FaqServiceQuery : IFaqServiceQuery
{
    private readonly GameOnlineContext _context;
    public FaqServiceQuery(GameOnlineContext context)
    {
        _context = context;
    }

    public List<GetQuestionsViewModel> GetQuestionsForClient(int productId)
    {
        var qResult = (from q in _context.Questions
                join u in _context.Users on q.UserId equals u.Id
                join a in _context.FaqAnswers on q.Id equals a.QuestionId into aFull
                from a in aFull.DefaultIfEmpty()
                join uanswer in _context.Users on a.UserId equals uanswer.Id into UA
                from uanswer in UA.DefaultIfEmpty()
                where (q.ProductId == productId && q.IsConfirm == true)
                select new GetQuestionsViewModel
                {
                    CreationDate = q.CreationDate.ToPersianDate("ds dd ms Y"),
                    QuestionId = q.Id,
                    QuestionText = q.QuestionText,
                    UserName = u.Name,
                    getFAQAnswer = a != null ? new GetFAQAnswersViewModel
                    {
                        AnswerId = a.Id,
                        AnswerText = a.AnswerText,
                        CreationDate = a.CreationDate != null
                            ? a.CreationDate.ToPersianDate("ds dd ms Y")
                            : null,
                        UserName = uanswer != null ? uanswer.Name : null
                    } : null
                })
            .AsNoTracking()
            .ToList();

        return qResult;
    }
}