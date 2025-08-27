using GameOnline.Core.Services.ProductServices.ProductServicesClient;
using Microsoft.AspNetCore.Mvc;

namespace GameOnline.Web.ViewComponents;

public class GetProductForCategoryComponent : ViewComponent
{
    private readonly IProductServicesClient _productServicesClient;

    public GetProductForCategoryComponent(IProductServicesClient productServicesClient)
    {
        _productServicesClient = productServicesClient;
    }
    public async Task<IViewComponentResult> InvokeAsync(int categoryId)
    {
        return await Task.FromResult(View("GetProduct",_productServicesClient.GetProductForCategory(categoryId)));
    }
}