using GameOnline.Core.ViewModels.Comment_FAQ.FAQ.Client;

namespace GameOnline.Core.Services.Comment_FAQ.Client
{
    public interface IFaqServiceClient
    {
        List<GetQuestionsViewModel> GetQuestionsForClient(int productId);
    }
}
