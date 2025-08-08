namespace GameOnline.Core.ViewModels.ProductViewmodel.Client;

public class DetailProductViewmodel
{
    public GetDetailProductClientViewmodel? DetailProduct { get; set; }
    public List<GetProductGalleriesViewmodel>? GetProductGalleries { get; set; }
    public List<GetProductPriceClientViewmodel>? GetProductPrice { get; set; }
    public List<GetSellerClientViewmodel> GetSeller { get; set; }
    public GetReviewForClientViewmodel? GetReview { get; set; }
}