namespace GameOnline.Core.ViewModels.ProductViewmodel.Client
{
    public class GetProductForCategoryViewmodel
    {
        public string CategoryName { get; set; }
        public int ProductId { get; set; }
        public string ImageName { get; set; }
        public string FaTitle { get; set; }
        public GetProductPriceForProductViewmodel GetProductPrices { get; set; }
    }
    public class GetProductPriceForProductViewmodel
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? MainPrice { get; set; }
        public int? SpecialPrice { get; set; }
    }

}
