namespace GameOnline.Core.ViewModels.ProductViewmodel.Client;

public class DetailProductViewmodel
{
    public GetDetailProductClientViewmodel? DetailProduct { get; set; }
    public List<GetProductGalleriesViewmodel>? GetProductGalleries { get; set; }
    public List<GetProductPriceClientViewmodel>? GetProductPriceClient { get; set; }
    public List<GetSellerClientViewmodel> GetSellerClient { get; set; }

}