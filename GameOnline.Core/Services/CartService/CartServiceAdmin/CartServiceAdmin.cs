using GameOnline.Common.Core;
using GameOnline.Core.ExtenstionMethods;
using GameOnline.Core.ViewModels.CartViewmodel.Admin;
using GameOnline.DataBase.Context;
using GameOnline.DataBase.Entities.Carts;
using GameOnline.DataBase.Entities.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using ZarinpalSandbox;


namespace GameOnline.Core.Services.CartService.CartServiceAdmin;

public class CartServiceAdmin : ICartServiceAdmin
{
    private readonly GameOnlineContext _context;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IConfiguration _config;
    private readonly HttpClient _httpClient;
    public CartServiceAdmin(GameOnlineContext context, IHttpContextAccessor contextAccessor, IConfiguration config, HttpClient httpClient)
    {
        _context = context;
        _contextAccessor = contextAccessor;
        _config = config;
        _httpClient = httpClient;
    }
    public OperationResult<int> AddCart(AddCartViewmodel addCart)
    {
        var cart = FindCartByUserId(addCart.UserId); // ✅ اینجا مشکل داشت

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

        var productPrice = FindProductPriceById(addCart.ProductPriceId);
        if (productPrice == null)
            return OperationResult<int>.Error("محصول یافت نشد");

        var cartDetail = FindCartDetail(cart.Id, addCart.ProductPriceId);

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


    public OperationResult<int> FindCartIdByUserId(int userId)
    {
        var findcart = _context.Carts
            .Where(c => c.UserId == userId)
            .FirstOrDefault(c => c.OrderType == OrderType.Product_selection);

        return OperationResult<int>.Success(findcart.Id);
    }

    public Cart? FindCartByUserId(int userId)
    {
        return _context.Carts
            .Include(c => c.CartDetails)
            .FirstOrDefault(c => c.UserId == userId && c.OrderType == OrderType.Product_selection);
    }

    public CartDetail? FindCartDetail(int cartId, int productPriceId)
    {
        return _context.CartDetails
            .Where(x => x.CartId.Equals(cartId))
            .FirstOrDefault(x => x.ProductPriceId.Equals(productPriceId));
    }

    public ProductPrice? FindProductPriceById(int productPriceId)
    {
        return _context.ProductPrices
            .FirstOrDefault(x => x.Id == productPriceId);
    }

    public Cart? FindCartById(int cartId)
    {
        return _context.Carts
            .Include(c => c.CartDetails)
            .FirstOrDefault(c => c.Id == cartId && c.OrderType == OrderType.Product_selection);
    }

    public async Task<OperationResult<string>> Payment(int cartId)
    {
        var findCart = FindCartById(cartId);
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
        var findCart = FindCartById(cartId);
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
}