using GameOnline.Common.Core;
using GameOnline.Core.ViewModels.CategoryViewModels;

namespace GameOnline.Core.Services.CategoryServices.Commands;

public interface ICategoryServicesCommand
{
    OperationResult<int> CreateCategory(CreateCategoriesViewModels createCategory);
    OperationResult<int> EditCategory(EditCategoriesViewModels editCategory);
    OperationResult<int> RemoveCategory(RemoveCategoriesViewModels removeCategory);
    OperationResult<int> AddOrEditParentCategory(AddOrEditParentCategoryViewmodel addOrEdit);
}