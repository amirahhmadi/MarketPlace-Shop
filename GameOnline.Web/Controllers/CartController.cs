using System.Security.Claims;
using GameOnline.Core.ExtenstionMethods;
using GameOnline.Core.Services.AddressService.Queries;
using GameOnline.Core.Services.CartService.Commands;
using GameOnline.Core.Services.CartService.Queries;
using GameOnline.Core.ViewModels.CartViewmodel.Admin;
using GameOnline.Core.ViewModels.CartViewmodel.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameOnline.Web.Controllers
{
    public class CartController : BaseController
    {
        private readonly ICartServiceQuery _cartServiceQuery;
        private readonly ICartServiceCommand _cartServiceCommand;
        private readonly IAddressServiceQuery _addressServiceQuery;

        public CartController(ICartServiceQuery cartServiceQuery, ICartServiceCommand cartServiceCommand, IAddressServiceQuery addressServiceQuery)
        {
            _cartServiceQuery = cartServiceQuery;
            _cartServiceCommand = cartServiceCommand;
            _addressServiceQuery = addressServiceQuery;
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

            var result = _cartServiceCommand.AddCart(addCart);
            TempData[TempDataName.Result] = Newtonsoft.Json.JsonConvert.SerializeObject(result);

            return RedirectToAction(nameof(CartDetail));
        }

        [HttpGet]
        [Authorize]
        [Route("CartDetail")]
        public IActionResult CartDetail()
        {
            var findCart = _cartServiceQuery.GetCartDetails(CurrentUserId);
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

            _cartServiceCommand.UpdateCheckCart(checkCart);
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
            _cartServiceCommand.UpdateChangeCart(detailid, count);
            return new JsonResult("ok");
        }

        [HttpPost]
        [Route("RemoveDetail")]
        public async Task<IActionResult> RemoveDetail(int detailid)
        {
            await _cartServiceCommand.RemoveDetailAsync(detailid);
            return new JsonResult("ok");
        }

        [HttpGet]
        [Route("Shopping")]
        [Authorize]
        public IActionResult Shopping()
        {
            var findActiveAddress = _addressServiceQuery.GetCartForShopping(CurrentUserId);
            if (findActiveAddress == null)
            {
                SetSweetAlert("error", "خطا", "برای ادامه آدرسی را ثبت کنید");
                return RedirectToAction(nameof(CartDetail));
            }
            findActiveAddress.GetCartDetails = _cartServiceQuery.GetCartDetails(CurrentUserId);

            return View(findActiveAddress);
        }

        [HttpGet]
        [Authorize]
        [Route("Shopping-Pay")]
        public IActionResult ShoppingPay()
        {
            int UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var FindCart = _cartServiceQuery.GetCartDetails(UserId);
            List<GetCartDetailsViewmodel> CheckCart = new List<GetCartDetailsViewmodel>();

            if (FindCart.Count() <= 0)
                return View("EmptyCart");

            foreach (var item in FindCart)
            {
                var MaxUserCount = item.ProductMaxUserCount ?? item.ProductCount;
                if (item.CartCount > MaxUserCount || item.CartCount > item.ProductCount)
                {
                    CheckCart.Add(new GetCartDetailsViewmodel
                    {
                        CartDetailId = item.CartDetailId,
                        IsRemove = true,
                    });
                    item.Message = "موجودی این محصول کمتر از درخواست شماس";
                    item.DetailType = 1;
                }

                int? Special = PriceEx.Pricecheck(item.StartDisCount, item.EndDisCount, item.SpecialPrice);

                if (Special == null)
                {
                    if (item.Price > item.MainPrice)
                    {
                        CheckCart.Add(new GetCartDetailsViewmodel
                        {
                            CartDetailId = item.CartDetailId,
                            NewPrice = item.MainPrice,
                        });
                        item.Message = $"قیمت محصول به مقدار {(item.Price - item.MainPrice).ToString("N0")} تومان کاهش داشته است";
                        item.DetailType = 2;
                        item.Price = item.MainPrice;
                    }
                    else if (item.Price < item.MainPrice)
                    {
                        CheckCart.Add(new GetCartDetailsViewmodel
                        {
                            CartDetailId = item.CartDetailId,
                            NewPrice = item.MainPrice,
                        });
                        item.Message = $"قیمت محصول به مقدار {(item.MainPrice - item.Price).ToString("N0")} تومان افزایش داشته است";
                        item.DetailType = 1;
                        item.Price = item.MainPrice;
                    }
                }
                else
                {
                    if (item.Price > Special.Value)
                    {
                        CheckCart.Add(new GetCartDetailsViewmodel
                        {
                            CartDetailId = item.CartDetailId,
                            NewPrice = Special.Value,
                        });
                        item.Message = $"قیمت محصول به مقدار {(item.Price - Special.Value).ToString("N0")} تومان کاهش داشته است";
                        item.DetailType = 2;
                        item.Price = Special.Value;
                    }
                    else if (item.Price < Special.Value)
                    {
                        CheckCart.Add(new GetCartDetailsViewmodel
                        {
                            CartDetailId = item.CartDetailId,
                            NewPrice = Special.Value,
                        });
                        item.Message = $"قیمت محصول به مقدار {(Special.Value - item.Price).ToString("N0")} تومان افزایش داشته است";
                        item.DetailType = 1;
                        item.Price = Special.Value;
                    }
                }

            }

            if (CheckCart.Count() > 0)
                return RedirectToAction(nameof(CartDetail));

            return View(FindCart);
        }

        [HttpGet]
        [Authorize]
        [Route("PayMentZarinPal")]
        public async Task<IActionResult> PayMentZarinPal()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var findCartId = _cartServiceQuery.FindCartIdByUserId(userId);

            if (findCartId == null)
                return View("Fail");

            var result = await _cartServiceCommand.Payment(findCartId.Data);

            if (result != null && result.IsSuccess)
                return Redirect(result.Data); // لینک درگاه پرداخت
            else
                return View("Fail");
        }

        [HttpGet]
        [Route("veryfication-ZarinPal/{cartId}")]
        public async Task<IActionResult> Verification(int cartId)
        {
            string status = HttpContext.Request.Query["Status"];
            string authority = HttpContext.Request.Query["Authority"];

            if (string.IsNullOrEmpty(status) || string.IsNullOrEmpty(authority))
                return View("Fail"); // درخواست نامعتبر

            if (status.ToLower() != "ok")
                return View("Fail"); // کاربر پرداخت را لغو کرده

            // Verify واقعی تراکنش
            var result = await _cartServiceCommand.VerificationZarinPal(cartId, authority);

            if (result != null && result.IsSuccess)
            {
                // پرداخت موفق
                return View(); // صفحه موفقیت
            }
            else
            {
                // پرداخت ناموفق
                return View("Fail"); // صفحه خطا
            }
        }
    }
}
