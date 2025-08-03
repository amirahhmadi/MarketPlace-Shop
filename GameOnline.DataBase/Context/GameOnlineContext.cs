using System.Security.Cryptography.X509Certificates;
using GameOnline.DataBase.Entities.Brands;
using GameOnline.DataBase.Entities.Categories;
using GameOnline.DataBase.Entities.Colors;
using GameOnline.DataBase.Entities.Discounts;
using GameOnline.DataBase.Entities.Guarantees;
using GameOnline.DataBase.Entities.Products;
using GameOnline.DataBase.Entities.Properties;
using GameOnline.DataBase.Entities.Sliders;
using Microsoft.EntityFrameworkCore;

namespace GameOnline.DataBase.Context;

public class GameOnlineContext : DbContext
{
    public GameOnlineContext(DbContextOptions<GameOnlineContext> options) : base(options)
    {

    }

    public DbSet<Brand> Brands { get; set; }
    public DbSet<Guarantee> Guarantees { get; set; }
    public DbSet<Slider> Sliders { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<SubCategory> SubCategories { get; set; }
    public DbSet<Color> Colors { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductGallery> ProductGalleries { get; set; }
    public DbSet<ProductReview> ProductReviews { get; set; }
    public DbSet<PropertyGroup> PropertyGroups { get; set; }
    public DbSet<PropertyName> PropertyNames { get; set; }
    public DbSet<PropertyValue> PropertyValues { get; set; }
    public DbSet<PropertyProduct> PropertyProducts { get; set; }
    public DbSet<PropertyNameCategory> PropertyNameCategories { get; set; }
    public DbSet<Discount> Discounts { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Global Query Filters (Soft Delete)
        modelBuilder.Entity<Brand>().HasQueryFilter(x => x.IsRemove == false);
        modelBuilder.Entity<Guarantee>().HasQueryFilter(x => x.IsRemove == false);
        modelBuilder.Entity<Slider>().HasQueryFilter(x => x.IsRemove == false);
        modelBuilder.Entity<Category>().HasQueryFilter(x => x.IsRemove == false);
        modelBuilder.Entity<SubCategory>().HasQueryFilter(x => x.IsRemove == false);
        modelBuilder.Entity<Product>().HasQueryFilter(x => x.IsRemove == false);

        // Optional relations to Product for avoiding EF Core warnings
        modelBuilder.Entity<ProductGallery>()
            .HasOne(pg => pg.Product)
            .WithMany(p => p.ProductGalleries) // فرض بر اینکه این نام navigation هست
            .HasForeignKey(pg => pg.ProductId)
            .IsRequired(false);

        modelBuilder.Entity<ProductReview>()
            .HasOne(pr => pr.Product)
            .WithMany(p => p.ProductReviews) // فرض بر اینکه این navigation هست
            .HasForeignKey(pr => pr.ProductId)
            .IsRequired(false);

        modelBuilder.Entity<PropertyProduct>()
            .HasOne(pp => pp.Product)
            .WithMany(p => p.PropertyProducts) // فرض بر اینکه این navigation هست
            .HasForeignKey(pp => pp.ProductId)
            .IsRequired(false);

        // تغییر همه DeleteBehaviorها از Cascade به Restrict
        var cascadeFKs = modelBuilder.Model.GetEntityTypes()
            .SelectMany(t => t.GetForeignKeys())
            .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

        foreach (var fk in cascadeFKs)
        {
            fk.DeleteBehavior = DeleteBehavior.Restrict;
        }

        base.OnModelCreating(modelBuilder);
    }

}