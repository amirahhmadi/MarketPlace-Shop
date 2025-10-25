using GameOnline.Common.Core;
using GameOnline.Core.ExtenstionMethods;
using GameOnline.Core.ViewModels.CartViewmodel.Admin;
using GameOnline.Core.ViewModels.CartViewmodel.Client;
using GameOnline.DataBase.Context;
using GameOnline.DataBase.Entities.Carts;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using GameOnline.Core.Services.CartService.Queries;

namespace GameOnline.Core.Services.CartService.Commands;

public class CartServiceCommand : ICartServiceCommand
{
    private readonly GameOnlineContext _context;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IConfiguration _config;
    private readonly HttpClient _httpClient;
    private readonly ICartServiceQuery _cartQuery;
    public CartServiceCommand(GameOnlineContext context, IHttpContextAccessor contextAccessor, IConfiguration config, HttpClient httpClient,ICartServiceQuery cartQuery)
    {
        _context = context;
        _contextAccessor = contextAccessor;
        _config = config;
        _httpClient = httpClient;
        _cartQuery = cartQuery;
    }
    public async Task<OperationResult<string>> Payment(int cartId)
    {
        var findCart = _cartQuery.FindCartById(cartId);
        if (findCart == null || findCart.CartDetails == null || !findCart.CartDetails.Any())
            return OperationResult<string>.Error("سبد خرید خالی است");

        int amount = findCart.CartDetails.Sum(x => x.Price * x.Count);
        findCart.SumOrder = amount;
        await _context.SaveChangesAsync();

        string merchantId = _config["ZarinPal:MerchantId"];

        // ساختن آدرس callback با توجه به دامنه فعلی
        string scheme = _contextAccessor.HttpContext.Request.Scheme;
        string host = _contextAccessor.HttpContext.Request.Host.Value;
        string callbackUrl = $"{scheme}://{host}{_config["ZarinPal:CallbackUrl"]}{findCart.Id}";

        var requestData = new
        {
            merchant_id = merchantId,
            amount = amount,
            callback_url = callbackUrl,
            description = "خرید از سایت گیم آنلاین",
            metadata = new { email = "test@test.com", mobile = "09120000000" }
        };

        var content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("https://sandbox.zarinpal.com/pg/v4/payment/request.json", content);
        var responseString = await response.Content.ReadAsStringAsync();

        var result = JsonConvert.DeserializeObject<dynamic>(responseString);

        if (result?.data?.code == 100)
        {
            string authority = result.data.authority;
            return OperationResult<string>.Success($"https://sandbox.zarinpal.com/pg/StartPay/{authority}");
        }

        string errorMsg = result?.errors?.message?.ToString()
                          ?? (result?.errors?.validations?.ToString() ?? "خطای ناشناخته");
        return OperationResult<string>.Error(errorMsg);
    }

    public async Task<OperationResult<int>> VerificationZarinPal(int cartId, string authority)
    {
        var findCart = _cartQuery.FindCartById(cartId);
        if (findCart == null || findCart.CartDetails == null || !findCart.CartDetails.Any())
            return OperationResult<int>.Error("سبد خرید پیدا نشد");

        int amount = findCart.CartDetails.Sum(x => x.Price * x.Count);
        findCart.SumOrder = amount;
        await _context.SaveChangesAsync();

        string merchantId = _config["ZarinPal:MerchantId"];

        var verifyData = new
        {
            merchant_id = merchantId,
            amount = amount,
            authority = authority
        };

        var content = new StringContent(JsonConvert.SerializeObject(verifyData), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("https://sandbox.zarinpal.com/pg/v4/payment/verify.json", content);
        var responseString = await response.Content.ReadAsStringAsync();

        var result = JsonConvert.DeserializeObject<dynamic>(responseString);

        if (result?.data?.code == 100)
        {
            findCart.OrderType = OrderType.review_queue;
            _context.Update(findCart);
            await _context.SaveChangesAsync();

            int refId = result.data.ref_id;
            return OperationResult<int>.Success(refId);
        }

        string errorMsg = result?.errors?.message?.ToString()
                          ?? (result?.errors?.validations?.ToString() ?? "خطای ناشناخته در وریفای");
        return OperationResult<int>.Error(errorMsg);
    }

    public OperationResult<int> AddCart(AddCartViewmodel addCart)
    {
        var cart = _cartQuery.FindCartByUserId(addCart.UserId); // ✅ اینجا مشکل داشت

        if (cart == null)
        {
            cart = new Cart
            {
                UserId = addCart.UserId,
                CreationDate = DateTime.Now,
                OrderType = OrderType.Product_selection,
            };
            _context.Carts.Add(cart);
            _context.SaveChanges();
        }
        else
        {
            cart.LastModified = DateTime.Now;
            // نیازی به Update نیست چون EF داره track می‌کنه
        }

        var productPrice = _cartQuery.FindProductPriceById(addCart.ProductPriceId);
        if (productPrice == null)
            return OperationResult<int>.Error("محصول یافت نشد");

        var cartDetail = _cartQuery.FindCartDetail(cart.Id, addCart.ProductPriceId);

        if (cartDetail == null)
        {
            cartDetail = new CartDetail
            {
                Price = productPrice.Price,
                ProductPriceId = productPrice.Id,
                CartId = cart.Id,
                Count = 1,
                CreationDate = DateTime.Now
            };
            _context.CartDetails.Add(cartDetail);
        }
        else
        {
            if (productPrice.Count > cartDetail.Count &&
                productPrice.MaxOrderCount > cartDetail.Count)
            {
                cartDetail.Count++;
                cartDetail.LastModified = DateTime.Now;
                cartDetail.Price = productPrice.Price;
            }
        }

        _context.SaveChanges();
        return OperationResult<int>.Success(addCart.UserId);
    }
    public async Task RemoveDetailAsync(int detailId)
    {
        var findDetail = await _context.CartDetails
            .FirstOrDefaultAsync(x => x.Id == detailId);

        if (findDetail == null) return;

        _context.CartDetails.Remove(findDetail);
        await _context.SaveChangesAsync();
    }

    public void UpdateChangeCart(int detailId, int count)
    {
        var findDetail = _context.CartDetails
            .FirstOrDefault(x => x.Id.Equals(detailId));

        findDetail.Count = count;
        _context.Update(findDetail);
        _context.SaveChanges();
    }

    public OperationResult<int> UpdateCheckCart(List<GetCartDetailsViewmodel> details)
    {
        var productPriceId = details.Select(x => x.CartDetailId).ToList();

        var findProductPrice = _context.CartDetails
            .Where(x => productPriceId.Contains(x.Id))
            .AsNoTracking().ToList();

        foreach (var item in findProductPrice)
        {
            var detail = details.Where(x => x.CartDetailId == item.Id).FirstOrDefault();
            if (detail.IsRemove)
            {
                _context.CartDetails.Remove(item);
            }
            else
            {
                item.Price = detail.NewPrice.Value;
                _context.CartDetails.Update(item);
            }

        }
        _context.SaveChanges();
        return OperationResult<int>.Success(1);
    }
}