using GameOnline.Common.Core;
using GameOnline.Core.ExtenstionMethods;
using GameOnline.Core.Services.CategoryServices.Queries;
using GameOnline.Core.ViewModels.CategoryViewModels;
using GameOnline.DataBase.Context;
using GameOnline.DataBase.Entities.Categories;

namespace GameOnline.Core.Services.CategoryServices.Commands;

public class CategoryServicesCommand : ICategoryServicesCommand
{
    private readonly GameOnlineContext _context;
    private readonly ICategoryServicesQuery _servicesQuery;

    public CategoryServicesCommand(GameOnlineContext context,ICategoryServicesQuery servicesQuery)
    {
        _context = context;
        _servicesQuery = servicesQuery;
    }

    public OperationResult<int> CreateCategory(CreateCategoriesViewModels createCategory)
    {
        if (_servicesQuery.IsCategoryExist(createCategory.FaTitle, createCategory.EnTitle, 0))
        {
            return OperationResult<int>.Duplicate();
        }

        string imageName = createCategory.ImageName.UploadImage(PathTools.PathCategoryImageAdmin);

        Category category = new Category()
        {
            CreationDate = DateTime.Now,
            Description = createCategory.Description,
            EnTitle = createCategory.EnTitle.ToLower().Trim(),
            FaTitle = createCategory.FaTitle.ToLower().Trim(),
            ImageName = imageName,
            IsActive = createCategory.IsActive,
            IsMine = createCategory.IsMine

        };
        _context.Categories.Add(category);
        _context.SaveChanges();
        return OperationResult<int>.Success(category.Id);
    }

    public OperationResult<int> EditCategory(EditCategoriesViewModels editCategory)
    {
        var category = _context.Categories
            .FirstOrDefault(x => x.Id == editCategory.CategoryId);
        if (category == null)
        {
            return OperationResult<int>.Error();
        }


        if (_servicesQuery.IsCategoryExist(editCategory.FaTitle, editCategory.EnTitle, editCategory.CategoryId))
        {
            return OperationResult<int>.Duplicate();
        }

        if (editCategory.ImageName != null)
        {
            ImageExtension.RemoveImage(category.ImageName, PathTools.PathCategoryImageAdmin);
            category.ImageName = editCategory.ImageName.UploadImage(PathTools.PathCategoryImageAdmin);
        }

        category.LastModified = DateTime.Now;
        category.Description = editCategory.Description;
        category.EnTitle = editCategory.EnTitle.ToLower().Trim();
        category.FaTitle = editCategory.FaTitle.ToLower().Trim();
        category.IsActive = editCategory.IsActive;
        category.IsMine = editCategory.IsMine;
        category.Icon = editCategory.Icon;


        _context.Categories.Update(category);
        _context.SaveChanges();
        return OperationResult<int>.Success(category.Id);
    }

    public OperationResult<int> AddOrEditParentCategory(AddOrEditParentCategoryViewmodel addOrEdit)
    {
        List<SubCategory> addParent = new List<SubCategory>();
        List<SubCategory> removeParent = new List<SubCategory>();
        List<SubCategory> oldParent = _servicesQuery.GetAllParentBySubId(addOrEdit.SubId);

        if (oldParent.Count() > 0)
        {
            foreach (var item in oldParent)
            {
                if (addOrEdit.ParentId != null)
                {
                    if (addOrEdit.ParentId.Contains(item.ParentId))
                    {
                        addOrEdit.ParentId.Remove(item.ParentId);
                    }
                    else
                    {
                        removeParent.Add(new SubCategory
                        {
                            Id = item.Id,
                        });
                    }
                }
                else
                {
                    removeParent.Add(new SubCategory
                    {
                        Id = item.Id,
                    });
                }


            }
            _context.SubCategories.RemoveRange(removeParent);
        }

        if (addOrEdit.ParentId != null)
        {
            foreach (var item in addOrEdit.ParentId)
            {
                if (oldParent.Any(x => x.SubId == addOrEdit.SubId
                                       && x.ParentId == item) == false)
                {
                    addParent.Add(new SubCategory
                    {
                        SubId = addOrEdit.SubId,
                        ParentId = item,
                        CreationDate = DateTime.Now,

                    });
                }
            }
        }

        _context.SubCategories.AddRange(addParent);
        _context.SaveChanges();

        return OperationResult<int>.Success(addOrEdit.SubId);
    }

    public OperationResult<int> RemoveCategory(RemoveCategoriesViewModels removeCategory)
    {
        var category = _context.Categories
            .FirstOrDefault(x => x.Id == removeCategory.CategoryId);

        if (category == null)
        {
            return OperationResult<int>.Error();
        }

        category.RemoveDate = DateTime.Now;
        category.IsRemove = true;

        _context.Categories.Update(category);
        _context.SaveChanges();

        return OperationResult<int>.Success(category.Id);
    }
}