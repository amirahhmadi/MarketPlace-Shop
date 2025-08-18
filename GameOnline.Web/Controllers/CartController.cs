using System.Security.Claims;
using GameOnline.Core.ExtenstionMethods;
using GameOnline.Core.Services.CartService.CartServiceAdmin;
using GameOnline.Core.Services.CartService.CartServiceClient;
using GameOnline.Core.ViewModels.CartViewmodel.Admin;
using GameOnline.Core.ViewModels.CartViewmodel.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameOnline.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartServiceAdmin _cartServiceAdmin;
        private readonly ICartServiceClient _cartServiceClient;

        public CartController(ICartServiceAdmin cartServiceAdmin, ICartServiceClient cartServiceClient)
        {
            _cartServiceAdmin = cartServiceAdmin;
            _cartServiceClient = cartServiceClient;
        }

        // گرفتن یوزر لاگین‌شده
        private int CurrentUserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        [HttpGet]
        [Authorize]
        [Route("/AddCart/{productPriceId}")]
        public IActionResult AddCart(int productPriceId)
        {
            AddCartViewmodel addCart = new AddCartViewmodel
            {
                UserId = CurrentUserId,
                ProductPriceId = productPriceId
            };

            var result = _cartServiceAdmin.AddCart(addCart);
            TempData[TempDataName.Result] = Newtonsoft.Json.JsonConvert.SerializeObject(result);

            return RedirectToAction(nameof(CartDetail));
        }

        [HttpGet]
        [Authorize]
        [Route("CartDetail")]
        public IActionResult CartDetail()
        {
            var findCart = _cartServiceClient.GetCartDetails(CurrentUserId);
            if (!findCart.Any())
                return View("EmptyCart");

            List<GetCartDetailsViewmodel> checkCart = new();

            foreach (var item in findCart)
            {
                var maxUserCount = item.ProductMaxUserCount ?? item.ProductCount;

                // بررسی تعداد
                if (item.CartCount > maxUserCount)
                {
                    checkCart.Add(new GetCartDetailsViewmodel
                    {
                        CartDetailId = item.CartDetailId,
                        IsRemove = true
                    });
                    item.Message = "موجودی این محصول کمتر از درخواست شماست";
                    item.DetailType = 1;
                }

                // بررسی قیمت / تخفیف
                int? special = PriceEx.Pricecheck(item.StartDisCount, item.EndDisCount, item.SpecialPrice);
                int newPrice = special ?? item.MainPrice;

                CheckAndUpdatePrice(item, newPrice, checkCart);
            }

            _cartServiceClient.UpdateCheckCart(checkCart);
            return View(findCart);
        }

        // متد کمکی برای تغییر قیمت
        private void CheckAndUpdatePrice(GetCartDetailsViewmodel item, int newPrice, List<GetCartDetailsViewmodel> checkCart)
        {
            if (item.Price > newPrice)
            {
                checkCart.Add(new GetCartDetailsViewmodel
                {
                    CartDetailId = item.CartDetailId,
                    NewPrice = newPrice
                });
                item.Message = $"قیمت محصول به مقدار {(item.Price - newPrice).ToString("N0")} تومان کاهش داشته است";
                item.DetailType = 2;
                item.Price = newPrice;
            }
            else if (item.Price < newPrice)
            {
                checkCart.Add(new GetCartDetailsViewmodel
                {
                    CartDetailId = item.CartDetailId,
                    NewPrice = newPrice
                });
                item.Message = $"قیمت محصول به مقدار {(newPrice - item.Price).ToString("N0")} تومان افزایش داشته است";
                item.DetailType = 1;
                item.Price = newPrice;
            }
        }

        [HttpPost]
        [Route("UpdateCart")]
        public IActionResult UpdateCart(int count, int detailid)
        {
            _cartServiceClient.UpdateChangeCart(detailid, count);
            return new JsonResult("ok");
        }

        [HttpPost]
        [Route("RemoveDetail")]
        public async Task<IActionResult> RemoveDetail(int detailid)
        {
            await _cartServiceClient.RemoveDetailAsync(detailid);
            return new JsonResult("ok");
        }
    }
}
