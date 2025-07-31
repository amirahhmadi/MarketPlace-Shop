using System.Security.Cryptography.X509Certificates;
using GameOnline.Common.Core;
using GameOnline.Core.ExtenstionMethods;
using GameOnline.Core.ViewModels.CategoryViewModels;
using GameOnline.DataBase.Context;
using GameOnline.DataBase.Entities.Categories;
using Microsoft.EntityFrameworkCore;

namespace GameOnline.Core.Services.CategoryServices.CategoryServicesAdmin
{
    public class CategoryServiceAdmin : ICategoryServiceAdmin
    {
        private readonly GameOnlineContext _context;

        public CategoryServiceAdmin(GameOnlineContext context)
        {
            _context = context;
        }
        public OperationResult<int> CreateCategory(CreateCategoriesViewModels createCategory)
        {
            if (IsCategoryExist(createCategory.FaTitle, createCategory.EnTitle, 0))
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


            if (IsCategoryExist(editCategory.FaTitle, editCategory.EnTitle, editCategory.CategoryId))
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

        public bool IsCategoryExist(string faTitle, string enTitle, int excludeId)
        {
            return _context.Categories.Any(x =>
                (x.FaTitle == faTitle.Trim() || x.EnTitle == enTitle.Trim()) &&
                x.Id != excludeId);
        }

        public OperationResult<int> AddOrEditParentCategory(AddOrEditParentCategoryViewmodel addOrEdit)
        {
            List<SubCategory> addParent = new List<SubCategory>();
            List<SubCategory> removeParent = new List<SubCategory>();
            List<SubCategory> oldParent = GetAllParentBySubId(addOrEdit.SubId);

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

        public List<SubCategory> GetAllParentBySubId(int subId)
        {
            return _context.SubCategories
                .Where(x => x.SubId == subId)
                .AsNoTracking()
                .ToList();
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
    }
}
