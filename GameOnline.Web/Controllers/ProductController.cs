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

        [HttpGet("/Detail/{productId}")]
        public IActionResult Detail(int productId)
        {
            var detail = new DetailProductViewmodel
            {
                DetailProduct = _productServicesClient.GetDetailProductById(productId)
            };

            if (detail.DetailProduct == null)
                return NotFound();

            detail.GetProductGalleries = _productServicesClient.GetProductGalleries(productId);
            detail.GetProductPriceClient = _productServicesClient.GetProductPriceClient(productId);
            detail.GetSellerClient = _productServicesClient.GetSellerForProductById(detail.GetProductPriceClient.GroupBy(x=>x.SellerId).Select(x=>x.Key).ToList());

            if (detail.GetProductPriceClient is null || detail.GetProductPriceClient.Count <= 0)
                return View("~/Views/Product/NoProduct.cshtml");

            return View(detail);
        }
    }
}
