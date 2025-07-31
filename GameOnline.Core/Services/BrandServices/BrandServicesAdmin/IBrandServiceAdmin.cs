using GameOnline.Common.Core;
using GameOnline.Core.ViewModels.BrandViewModels;

namespace GameOnline.Core.Services.BrandServices.BrandServicesAdmin;

public interface IBrandServiceAdmin
{
    List<GetBrandsViewModel> GetBrands(); // نمایش برند ها
    OperationResult<int> CreateBrand(CreateBrandsViewModel createBrand); // ساخت برند
    EditBrandsViewModel GetBrandById(int BrandId); // نمایش برند بر اساس ایدی
    OperationResult<int> EditBrand(EditBrandsViewModel editBrand); // ویرایش برند
    OperationResult<int> RemoveBrand(RemoveBrandsViewModel removeBrand); // حذف برند
}