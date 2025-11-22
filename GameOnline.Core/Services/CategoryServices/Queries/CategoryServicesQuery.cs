using GameOnline.Core.ViewModels.CategoryViewModels;
using GameOnline.DataBase.Context;
using GameOnline.DataBase.Entities.Categories;
using Microsoft.EntityFrameworkCore;

namespace GameOnline.Core.Services.CategoryServices.Queries;

public class CategoryServicesQuery : ICategoryServicesQuery
{
    private readonly GameOnlineContext _context;

    public CategoryServicesQuery(GameOnlineContext context)
    {
        _context = context;
    }

    public List<GetCategoriesViewModels> GetCategory()
    {
        return _context.Categories
            .Select(x => new GetCategoriesViewModels
            {
                CategoryId = x.Id,
                EnTitle = x.EnTitle,
                FaTitle = x.FaTitle,
                ImageName = x.ImageName,
                IsActive = x.IsActive,
                IsMine = x.IsMine
            })
            .AsNoTracking()
            .ToList();
    }

    public EditCategoriesViewModels? GetCategoryById(int categoryId)
    {
        return _context.Categories
            .Where(x => x.Id == categoryId)
            .Select(x => new EditCategoriesViewModels
            {
                CategoryId = x.Id,
                Description = x.Description,
                EnTitle = x.EnTitle,
                FaTitle = x.FaTitle,
                Icon = x.Icon,
                IsActive = x.IsActive,
                IsMine = x.IsMine,
                OldImage = x.ImageName,

            })
            .FirstOrDefault();
    }

    public List<GetCategoriesForParentListViewmodel> GetCategoriesForParentList(int subId)
    {
        return _context.Categories
            .Where(x => x.Id != subId)
            .Select(x => new GetCategoriesForParentListViewmodel()
            {
                CategoryId = x.Id,
                EnTitle = x.EnTitle,
                FaTitle = x.FaTitle,
                ImageName = x.ImageName,
                IsActive = x.IsActive,
                IsMine = x.IsMine
            }).AsNoTracking().ToList();
    }

    public List<GetParentCategoryForAddOrRemoveSubViewmodel> GetParentCategoryForAddOrRemoveSub(int subId)
    {
        var q = (from c in _context.Categories
                 join s in _context.SubCategories on c.Id equals s.ParentId

                 where s.SubId == subId

                 select new GetParentCategoryForAddOrRemoveSubViewmodel
                 {
                     CategoryId = c.Id,
                     FaTitle = c.FaTitle,
                 })
            .AsNoTracking()
            .ToList();
        return q;
    }

    public List<SubCategory> GetAllParentBySubId(int subId)
    {
        return _context.SubCategories
            .Where(x => x.SubId == subId)
            .AsNoTracking()
            .ToList();
    }

    public bool IsCategoryExist(string faTitle, string enTitle, int excludeId)
    {
        return _context.Categories.Any(x =>
            (x.FaTitle == faTitle.Trim() || x.EnTitle == enTitle.Trim()) &&
            x.Id != excludeId);
    }

    public List<GetCategoriesForMenuViewmodel> GetCategoriesForMenu()
    {
        var menu = (from c in _context.Categories
                    join sub in _context.SubCategories on c.Id equals sub.SubId
                    into ss from sub in ss.DefaultIfEmpty()
                    where c.IsActive == true && c.IsRemove == false
                    select new GetCategoriesForMenuViewmodel()
                    {
                        CategoryId = c.Id,
                        ParentId = sub.ParentId,
                        FaTitle = c.FaTitle,
                        IsMine = c.IsMine
                    }).ToList();
        return menu;
    }
}