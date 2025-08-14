using GameOnline.Core.Services.ProductServices.ProductServicesClient;
using GameOnline.Core.ViewModels.ProductViewmodel.Client;
using GameOnline.DataBase.Entities.Products;
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
            detail.GetProductPrice = _productServicesClient.GetProductPriceClient(productId);
            detail.GetSeller = _productServicesClient.GetSellerForProductById(detail.GetProductPrice.GroupBy(x=>x.SellerId).Select(x=>x.Key).ToList());
            detail.GetReview = _productServicesClient.GetReviewForClient(productId);
            detail.GetProperty = _productServicesClient.GetPropertyForProductClient(productId);

            if (detail.GetProductPrice is null || detail.GetProductPrice.Count <= 0)
                return View("~/Views/Product/NoProduct.cshtml", detail);

            return View(detail);
        }


        [HttpPost]
        [Route("PropertyProduct/{ProductId}/{Producten}")]
        public IActionResult PropertyProduct(int ProductId, string Producten)
        {
            TempData[ProductEn] = Producten;
            return View(_productServicesClient.GetPropertyForProductClient(ProductId));
        }
    }
}
