using System.Runtime.CompilerServices;
using GameOnline.Common.Core;
using GameOnline.Core.ExtenstionMethods;
using GameOnline.Core.ViewModels.BrandViewModels;
using GameOnline.Core.ViewModels.ProductViewmodel.Admin;
using GameOnline.Core.ViewModels.ProductViewmodel.Client;
using GameOnline.DataBase.Context;
using GameOnline.DataBase.Entities.Brands;
using GameOnline.DataBase.Entities.Products;
using Microsoft.EntityFrameworkCore;

namespace GameOnline.Core.Services.ProductServices.ProductServicesAdmin;

public class ProductServicesAdmin : IProductServicesAdmin
{
    private readonly GameOnlineContext _context;

    public ProductServicesAdmin(GameOnlineContext context)
    {
        _context = context;
    }

    public OperationResult<int> CreateProduct(CreateProductViewmodel createProduct)
    {
        if (IsProductExist(createProduct.FaTitle, createProduct.EnTitle, 0))
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

        if (IsProductExist(editProduct.FaTitle, editProduct.EnTitle, editProduct.ProductId))
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

    public AddOrUpdateProductReviewViewmodel? FindProductReviewById(int productId)
    {
        return _context.ProductReviews
            .Where(x => x.ProductId == productId)
            .Select(x => new AddOrUpdateProductReviewViewmodel()
            {
                Negative = x.Negative,
                Positive = x.Positive,
                ProductId = productId,
                Review = x.Review,
                ReviewId = x.Id
            }).AsNoTracking().FirstOrDefault();
    }

    public EditProductViewmodel? GetProductById(int productId)
    {
        return _context.Products
            .Where(x => x.Id == productId)
            .Select(x => new EditProductViewmodel()
            {
                ProductId = x.Id,
                BrandId = x.BrandId,
                CategoryId = x.CategoryId,
                FaTitle = x.FaTitle,
                EnTitle = x.EnTitle,
                IsActive = x.IsActive,
                Score = x.Score,
                OldImage = x.ImageName
            }).AsNoTracking().FirstOrDefault();
    }

    public List<GetProductViewmodel> GetProducts()
    {
        var product = (from p in _context.Products
                       join b in _context.Brands on p.BrandId equals b.Id
                       join c in _context.Categories on p.CategoryId equals c.Id

                       select new GetProductViewmodel()
                       {
                           BrandName = b.FaTitle,
                           CategoryName = c.FaTitle,
                           FaTitle = p.FaTitle,
                           ImageName = p.ImageName,
                           ProductId = p.Id,
                           IsActive = p.IsActive

                       }).AsNoTracking().ToList();
        return product;
    }

    public bool IsProductExist(string faTitle, string enTitle, int excludeId)
    {
        return _context.Products.Any(x =>
            (x.FaTitle == faTitle.Trim() || x.EnTitle == enTitle.Trim()) &&
            x.Id != excludeId);
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
