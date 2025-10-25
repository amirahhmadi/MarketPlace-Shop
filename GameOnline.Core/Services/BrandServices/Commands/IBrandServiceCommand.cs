using GameOnline.Common.Core;
using GameOnline.Core.ViewModels.BrandViewModels;
using Microsoft.EntityFrameworkCore;

namespace GameOnline.Core.Services.BrandServices.Commands;

public interface IBrandServiceCommand
{
    OperationResult<int> CreateBrand(CreateBrandsViewModel createBrand); // ساخت برند
    OperationResult<int> EditBrand(EditBrandsViewModel editBrand); // ویرایش برند
    OperationResult<int> RemoveBrand(RemoveBrandsViewModel removeBrand); // حذف برند
}