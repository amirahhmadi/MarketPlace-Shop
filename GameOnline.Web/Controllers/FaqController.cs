using GameOnline.Core.Services.Comment_FAQ.Queries.FAQ;
using GameOnline.Core.ViewModels.Comment_FAQ.FAQ.Client;
using Microsoft.AspNetCore.Mvc;

namespace GameOnline.Web.Controllers
{
    public class FaqController : BaseController
    {
        private readonly IFaqServiceQuery _faqServiceQuery;

        public FaqController(IFaqServiceQuery faqServiceQuery)
        {
            _faqServiceQuery = faqServiceQuery;
        }

        [HttpPost]
        [Route("Question/{productId}/{Producten}")]
        public IActionResult Question(int productId, string Producten)
        {
            TempData[ProductEn] = Producten;
            return View(_faqServiceQuery.GetQuestionsForClient(productId));
        }
    }
}
