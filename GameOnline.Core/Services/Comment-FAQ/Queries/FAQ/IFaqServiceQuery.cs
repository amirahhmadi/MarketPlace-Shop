using GameOnline.Core.ViewModels.Comment_FAQ.FAQ.Client;

namespace GameOnline.Core.Services.Comment_FAQ.Queries.FAQ;

public interface IFaqServiceQuery
{
    List<GetQuestionsViewModel> GetQuestionsForClient(int productId);
}