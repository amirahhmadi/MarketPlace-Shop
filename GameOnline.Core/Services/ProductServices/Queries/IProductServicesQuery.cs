using GameOnline.Core.ViewModels.ProductViewmodel.Admin;
using GameOnline.Core.ViewModels.ProductViewmodel.Client;

namespace GameOnline.Core.Services.ProductServices.Queries;

public interface IProductServicesQuery
{
    GetDetailProductClientViewmodel? GetDetailProductById(int productId);
    List<GetProductGalleriesViewmodel> GetProductGalleries(int productId);
    List<GetProductPriceClientViewmodel> GetProductPriceClient(int productId);
    List<GetSellerClientViewmodel> GetSellerForProductById(List<int> sellerId);
    GetReviewForClientViewmodel? GetReviewForClient(int productId);
    List<GetPropertyForProductClientViewmodel> GetPropertyForProductClient(int productId);
    List<GetProductForCategoryViewmodel> GetProductForCategory(int categoryId);
    List<GetProductForCategoryViewmodel> GetDiscountedProducts();
    List<GetProductViewmodel> GetProducts();
    EditProductViewmodel? GetProductById(int productId);
    AddOrUpdateProductReviewViewmodel? FindProductReviewById(int productId);
    bool IsProductExist(string faTitle, string enTitle, int excludeId);
}