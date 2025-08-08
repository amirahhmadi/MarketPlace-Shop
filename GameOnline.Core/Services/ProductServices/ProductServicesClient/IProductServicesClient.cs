using GameOnline.Core.ViewModels.ProductViewmodel.Client;

namespace GameOnline.Core.Services.ProductServices.ProductServicesClient;

public interface IProductServicesClient
{
    GetDetailProductClientViewmodel? GetDetailProductById(int productId);
    List<GetProductGalleriesViewmodel> GetProductGalleries(int productId);
    List<GetProductPriceClientViewmodel> GetProductPriceClient(int productId);
    List<GetSellerClientViewmodel> GetSellerForProductById(List<int> sellerId);
    GetReviewForClientViewmodel? GetReviewForClient(int productId);
    List<GetPropertyForProductClientViewmodel> GetPropertyForProductClient(int productId);

}