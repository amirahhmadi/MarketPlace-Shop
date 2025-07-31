using GameOnline.Common.Core;
using GameOnline.Core.ViewModels.CategoryViewModels;
using GameOnline.Core.ViewModels.CategoryViewModels;

namespace GameOnline.Core.Services.CategoryServices.CategoryServicesAdmin
{
    public interface ICategoryServiceAdmin
    {
        List<GetCategoriesViewModels> GetCategory();
        OperationResult<int> CreateCategory(CreateCategoriesViewModels createCategory);
        EditCategoriesViewModels? GetCategoryById(int categoryId);
        OperationResult<int> EditCategory(EditCategoriesViewModels editCategory);
        OperationResult<int> RemoveCategory(RemoveCategoriesViewModels removeCategory);
        OperationResult<int> AddOrEditParentCategory(AddOrEditParentCategoryViewmodel addOrEdit);
        List<GetCategoriesForParentListViewmodel> GetCategoriesForParentList(int subId);
        List<GetParentCategoryForAddOrRemoveSubViewmodel> GetParentCategoryForAddOrRemoveSub(int subId);
    }
}