using GameOnline.Core.Services.Comment_FAQ.Client;
using GameOnline.Core.ViewModels.Comment_FAQ.FAQ.Client;
using Microsoft.AspNetCore.Mvc;

namespace GameOnline.Web.Controllers
{
    public class FaqController : BaseController
    {
        private readonly IFaqServiceClient _faqServiceClient;
        public FaqController(IFaqServiceClient faqServiceClient)
        {
            _faqServiceClient = faqServiceClient;
        }

        [HttpPost]
        [Route("Question/{productId}/{Producten}")]
        public IActionResult Question(int productId, string Producten)
        {
            TempData[ProductEn] = Producten;
            return View(_faqServiceClient.GetQuestionsForClient(productId));
        }
    }
}
