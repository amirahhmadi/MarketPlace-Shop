using System.Security.Claims;
using GameOnline.Core.ExtenstionMethods;
using GameOnline.Core.Security;
using GameOnline.Core.Services.ProductServices.Commands;
using GameOnline.Core.Services.ProductServices.Queries;
using GameOnline.Core.ViewModels.ProductViewmodel.Client;
using GameOnline.DataBase.Entities.Products;
using Microsoft.AspNetCore.Mvc;

namespace GameOnline.Web.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IProductServicesQuery _productServicesQuery;
        private readonly IProductServicesCommand _productServicesCommand;

        public ProductController(IProductServicesQuery productServicesQuery, IProductServicesCommand productServicesCommand)
        {
            _productServicesQuery = productServicesQuery;
            _productServicesCommand = productServicesCommand;
        }

        [HttpGet("/Detail/{productId}")]
        public IActionResult Detail(int productId)
        {
            var detail = new DetailProductViewmodel
            {
                DetailProduct = _productServicesQuery.GetDetailProductById(productId)
            };

            if (detail.DetailProduct == null)
                return NotFound();

            detail.GetProductGalleries = _productServicesQuery.GetProductGalleries(productId);
            detail.GetProductPrice = _productServicesQuery.GetProductPriceClient(productId);

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

            detail.GetSeller = _productServicesQuery.GetSellerForProductById(
                detail.GetProductPrice
                    .Select(x => x.SellerId)
                    .Distinct()
                    .ToList()
            );

            detail.GetReview = _productServicesQuery.GetReviewForClient(productId);
            detail.GetProperty = _productServicesQuery.GetPropertyForProductClient(productId);

            return View(detail);
        }


        [HttpPost]
        [Route("PropertyProduct/{ProductId}/{Producten}")]
        public IActionResult PropertyProduct(int ProductId, string Producten)
        {
            TempData[ProductEn] = Producten;
            return View(_productServicesQuery.GetPropertyForProductClient(ProductId));
        }

        [HttpPost, Route("AddOrRemoveFaviorate")]
        public IActionResult AddOrRemoveFavorite(int productId)
        {
            if (User.Identity.IsAuthenticated == false)
            {
                return Json(false);
            }

            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var result = _productServicesCommand.AddProductFavorite(userId, productId);

            return Json(result);
        }

        [HttpPost]
        [Route("CheckFaviorateProduct")]
        public IActionResult CheckFaviorateProduct(int productId)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var check = _productServicesQuery.CheckFavoriteProduct(userId, productId);

            return Json(check.Data);
        }
    }
}
