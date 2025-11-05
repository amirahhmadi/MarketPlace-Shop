using GameOnline.Common.Core;
using GameOnline.Core.ExtenstionMethods;
using GameOnline.Core.Services.ProductServices.Queries;
using GameOnline.Core.ViewModels.ProductViewmodel.Admin;
using GameOnline.DataBase.Context;
using GameOnline.DataBase.Entities.Products;

namespace GameOnline.Core.Services.ProductServices.Commands;

public class ProductServicesCommand : IProductServicesCommand
{
    private readonly GameOnlineContext _context;
    private readonly IProductServicesQuery _servicesQuery;

    public ProductServicesCommand(GameOnlineContext context, IProductServicesQuery servicesQuery)
    {
        _context = context;
        _servicesQuery = servicesQuery;
    }

    public OperationResult<int> CreateProduct(CreateProductViewmodel createProduct)
    {
        if (_servicesQuery.IsProductExist(createProduct.FaTitle, createProduct.EnTitle, 0))
        {
            return OperationResult<int>.Duplicate();
        }

        string imageName = createProduct.ImageName.UploadImage(PathTools.PathProductImageAdmin);

        Product product = new Product()
        {
            BrandId = createProduct.BrandId,
            CategoryId = createProduct.CategoryId,
            EnTitle = createProduct.EnTitle,
            FaTitle = createProduct.FaTitle,
            CreationDate = DateTime.Now,
            IsActive = createProduct.IsActive,
            Score = createProduct.Score,
            ImageName = imageName

        };
        _context.Products.Add(product);
        _context.SaveChanges();
        return OperationResult<int>.Success(product.Id);
    }

    public OperationResult<int> EditProduct(EditProductViewmodel editProduct)
    {
        var product = _context.Products
            .FirstOrDefault(x => x.Id == editProduct.ProductId);

        if (product == null)
            return OperationResult<int>.NotFound();

        if (_servicesQuery.IsProductExist(editProduct.FaTitle, editProduct.EnTitle, editProduct.ProductId))
        {
            return OperationResult<int>.Duplicate();
        }

        if (editProduct.ImageName is { Length: > 0 })
        {
            ImageExtension.RemoveImage(product.ImageName, PathTools.PathProductImageAdmin);
            product.ImageName = editProduct.ImageName.UploadImage(PathTools.PathProductImageAdmin);
        }

        product.BrandId = editProduct.BrandId;
        product.CategoryId = editProduct.CategoryId;
        product.FaTitle = editProduct.FaTitle;
        product.EnTitle = editProduct.EnTitle;
        product.LastModified = DateTime.Now;
        product.IsActive = editProduct.IsActive;
        product.Score = editProduct.Score;

        _context.Products.Update(product);
        _context.SaveChanges();
        return OperationResult<int>.Success(editProduct.ProductId);
    }

    public OperationResult<int> EditProductReview(AddOrUpdateProductReviewViewmodel reviewViewmodel)
    {
        var productReview = _context.ProductReviews.FirstOrDefault(x => x.ProductId == reviewViewmodel.ProductId);

        if (productReview == null)
        {
            productReview = new ProductReview()
            {
                ProductId = reviewViewmodel.ProductId,
                CreationDate = DateTime.Now,
                Negative = reviewViewmodel.Negative,
                Positive = reviewViewmodel.Positive,
                Review = reviewViewmodel.Review
            };
            _context.ProductReviews.Add(productReview);
        }
        else
        {
            productReview.Review = reviewViewmodel.Review;
            productReview.Positive = reviewViewmodel.Positive;
            productReview.Negative = reviewViewmodel.Negative;
            productReview.LastModified = DateTime.Now;

            _context.ProductReviews.Update(productReview);
        }

        _context.SaveChanges();
        return OperationResult<int>.Success(productReview.Id);
    }

    public OperationResult<int> RemoveProduct(RemoveProductViewModel removeProduct)
    {
        var product = _context.Products
            .FirstOrDefault(x => x.Id == removeProduct.ProductId);

        if (product == null)
            return OperationResult<int>.NotFound();

        product.IsRemove = true;
        product.RemoveDate = DateTime.Now;

        _context.Products.Update(product);
        _context.SaveChanges();
        return OperationResult<int>.Success(removeProduct.ProductId);
    }
}