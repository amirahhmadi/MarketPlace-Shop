using GameOnline.Core.ViewModels.ProductViewmodel.Client;

namespace GameOnline.Core.Services.ProductServices.ProductServicesClient;

public interface IProductServicesClient
{
    GetDetailProductClientViewmodel? GetDetailProductById(int productId);
}