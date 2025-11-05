using GameOnline.Common.Core;
using GameOnline.Core.ViewModels.ProductViewmodel.Admin;

namespace GameOnline.Core.Services.ProductServices.Commands;

public interface IProductServicesCommand
{
    OperationResult<int> CreateProduct(CreateProductViewmodel createProduct);
    OperationResult<int> EditProduct(EditProductViewmodel editProduct);
    OperationResult<int> RemoveProduct(RemoveProductViewModel removeProduct);
    OperationResult<int> EditProductReview(AddOrUpdateProductReviewViewmodel reviewViewmodel);
}