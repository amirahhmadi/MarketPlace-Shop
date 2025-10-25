using GameOnline.Core.ViewModels.CategoryViewModels;
using GameOnline.DataBase.Entities.Categories;

namespace GameOnline.Core.Services.CategoryServices.Queries;

public interface ICategoryServicesQuery
{
    List<GetCategoriesViewModels> GetCategory();
    EditCategoriesViewModels? GetCategoryById(int categoryId);
    List<GetCategoriesForParentListViewmodel> GetCategoriesForParentList(int subId);
    List<GetParentCategoryForAddOrRemoveSubViewmodel> GetParentCategoryForAddOrRemoveSub(int subId);
    bool IsCategoryExist(string faTitle, string enTitle, int excludeId);
    List<SubCategory> GetAllParentBySubId(int subId);
}