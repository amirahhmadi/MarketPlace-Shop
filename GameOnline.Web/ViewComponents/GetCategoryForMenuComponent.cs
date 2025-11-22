using GameOnline.Core.Services.CategoryServices.Queries;
using GameOnline.Core.Services.ProductServices.Queries;
using Microsoft.AspNetCore.Mvc;

namespace GameOnline.Web.ViewComponents;

public class GetCategoryForMenuComponent : ViewComponent
{
    private readonly ICategoryServicesQuery _categoryServicesQuery;

    public GetCategoryForMenuComponent(ICategoryServicesQuery categoryServicesQuery)
    {
        _categoryServicesQuery = categoryServicesQuery;
    }
    public async Task<IViewComponentResult> InvokeAsync()
    {
        return await Task.FromResult(View("GetCategory",_categoryServicesQuery.GetCategoriesForMenu()));
    }
}