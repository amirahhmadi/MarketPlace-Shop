using GameOnline.Core.Services.ProductServices.Queries;
using Microsoft.AspNetCore.Mvc;

namespace GameOnline.Web.ViewComponents;

public class GetProductForCategoryComponent : ViewComponent
{
    private readonly IProductServicesQuery _productServicesQuery;

    public GetProductForCategoryComponent(IProductServicesQuery productServicesQuery)
    {
        _productServicesQuery = productServicesQuery;
    }
    public async Task<IViewComponentResult> InvokeAsync(int categoryId)
    {
        return await Task.FromResult(View("GetProduct",_productServicesQuery.GetProductForCategory(categoryId)));
    }
}