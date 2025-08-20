using GameOnline.Core.ExtenstionMethods;
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

            if (detail.GetProductPrice == null || !detail.GetProductPrice.Any())
                return View("~/Views/Product/NoProduct.cshtml", detail);

            foreach (var price in detail.GetProductPrice)
            {
                // بررسی تخفیف
                var special = PriceEx.Pricecheck(price.StartDisCount, price.EndDisCount, price.SpecialPrice);

                // اگر تخفیف معتبر بود FinalPrice = special
                // در غیر این صورت MainPrice
                price.FinalPrice = special ?? price.MainPrice;

                // پرچم تخفیف
                price.HasDiscount = special.HasValue;
            }

            detail.GetSeller = _productServicesClient.GetSellerForProductById(
                detail.GetProductPrice
                    .Select(x => x.SellerId)
                    .Distinct()
                    .ToList()
            );

            detail.GetReview = _productServicesClient.GetReviewForClient(productId);
            detail.GetProperty = _productServicesClient.GetPropertyForProductClient(productId);

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
