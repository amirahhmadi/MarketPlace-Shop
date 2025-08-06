using GameOnline.Core.Services.ProductServices.ProductServicesClient;
using GameOnline.Core.ViewModels.ProductViewmodel.Client;
using Microsoft.AspNetCore.Mvc;

namespace GameOnline.Web.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IProductServicesClient _productServicesClient;
        public ProductController(IProductServicesClient productServicesClient)
        {
            _productServicesClient = productServicesClient;
        }

        [Route("/Detail/{productId}")]
        public IActionResult Detail(int productId)
        {
            DetailProductViewmodel detail = new DetailProductViewmodel();
            detail.DetailProduct = _productServicesClient.GetDetailProductById(productId);
            return View(detail);
        }
    }
}
