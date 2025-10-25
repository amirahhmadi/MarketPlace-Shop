using GameOnline.Core.ViewModels.BrandViewModels;

namespace GameOnline.Core.Services.BrandServices.Queries;

public interface IBrandServiceQuery
{
    List<GetBrandsViewModel> GetBrands(); // نمایش برند ها
    EditBrandsViewModel GetBrandById(int brandId); // نمایش برند بر اساس ایدی
    bool IsBrandExist(string faTitle, string enTitle, int excludeId);
}