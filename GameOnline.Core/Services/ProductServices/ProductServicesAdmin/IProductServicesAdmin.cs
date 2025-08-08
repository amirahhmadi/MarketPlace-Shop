using GameOnline.Common.Core;
using GameOnline.Core.ViewModels.BrandViewModels;
using GameOnline.Core.ViewModels.ProductViewmodel.Admin;
using GameOnline.Core.ViewModels.ProductViewmodel.Client;

namespace GameOnline.Core.Services.ProductServices.ProductServicesAdmin;

public interface IProductServicesAdmin
{
    List<GetProductViewmodel> GetProducts();
    OperationResult<int> CreateProduct(CreateProductViewmodel createProduct);
    EditProductViewmodel? GetProductById(int productId);
    OperationResult<int> EditProduct(EditProductViewmodel editProduct);
    OperationResult<int> RemoveProduct(RemoveProductViewModel removeProduct);
    AddOrUpdateProductReviewViewmodel? FindProductReviewById(int productId);
    OperationResult<int> EditProductReview(AddOrUpdateProductReviewViewmodel reviewViewmodel);
}