using System.Security.Cryptography.X509Certificates;
using GameOnline.DataBase.Entities.Brands;
using GameOnline.DataBase.Entities.Categories;
using GameOnline.DataBase.Entities.Colors;
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


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Brand>().HasQueryFilter(x => x.IsRemove == false);
        modelBuilder.Entity<Guarantee>().HasQueryFilter(x => x.IsRemove == false);
        modelBuilder.Entity<Slider>().HasQueryFilter(x => x.IsRemove == false);
        modelBuilder.Entity<Category>().HasQueryFilter(x => x.IsRemove == false);
        modelBuilder.Entity<SubCategory>().HasQueryFilter(x => x.IsRemove == false);


        var cascade = modelBuilder.Model.GetEntityTypes()
            .SelectMany(x => x.GetForeignKeys())
            .Where(x => !x.IsOwnership && x.DeleteBehavior == DeleteBehavior.Cascade);

        foreach (var type in cascade)
            type.DeleteBehavior = DeleteBehavior.Restrict;


        base.OnModelCreating(modelBuilder);
    }
}