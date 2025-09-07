using GameOnline.DataBase.Entities.Address;
using GameOnline.DataBase.Entities.Brands;
using GameOnline.DataBase.Entities.Carts;
using GameOnline.DataBase.Entities.Categories;
using GameOnline.DataBase.Entities.Colors;
using GameOnline.DataBase.Entities.Comment_FAQ;
using GameOnline.DataBase.Entities.Discounts;
using GameOnline.DataBase.Entities.Guarantees;
using GameOnline.DataBase.Entities.Payment;
using GameOnline.DataBase.Entities.Products;
using GameOnline.DataBase.Entities.Properties;
using GameOnline.DataBase.Entities.Role;
using GameOnline.DataBase.Entities.Sellers;
using GameOnline.DataBase.Entities.Sliders;
using GameOnline.DataBase.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace GameOnline.DataBase.Context;

public class GameOnlineContext : DbContext
{
    public GameOnlineContext(DbContextOptions<GameOnlineContext> options) : base(options) { }

    // DbSets
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
    public DbSet<Seller> Sellers { get; set; }
    public DbSet<ProductPrice?> ProductPrices { get; set; }
    public DbSet<Cart?> Carts { get; set; }
    public DbSet<CartDetail?> CartDetails { get; set; }
    public DbSet<User?> Users { get; set; }
    public DbSet<PaymentDetail> PaymentDetails { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<FAQAnswer> FaqAnswers { get; set; }
    public DbSet<UserAddress> UserAddresses { get; set; }
    public DbSet<City> Cities { get; set; }
    public DbSet<Province> Provinces { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<RolePermission> RolePermissions { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // -------------------------
        // Global Query Filters (Soft Delete)
        // -------------------------
        modelBuilder.Entity<Brand>().HasQueryFilter(x => !x.IsRemove);
        modelBuilder.Entity<Guarantee>().HasQueryFilter(x => !x.IsRemove);
        modelBuilder.Entity<Slider>().HasQueryFilter(x => !x.IsRemove);
        modelBuilder.Entity<Category>().HasQueryFilter(x => !x.IsRemove);
        modelBuilder.Entity<SubCategory>().HasQueryFilter(x => !x.IsRemove);
        modelBuilder.Entity<Product>().HasQueryFilter(x => !x.IsRemove);

        modelBuilder.Entity<ProductPrice>().HasQueryFilter(pp =>
            !pp.IsRemove &&
            pp.Product != null && !pp.Product.IsRemove);

        modelBuilder.Entity<CartDetail>().HasQueryFilter(cd =>
            cd.ProductPrice != null && !cd.ProductPrice.IsRemove);

        // اگر لازم داری سوال‌های محصولات حذف‌شده نمایش داده نشن:
        modelBuilder.Entity<Question>().HasQueryFilter(q =>
            q.Product != null && !q.Product.IsRemove);

        // -------------------------
        // Optional Relations (برای رفع EF Warning 10622)
        // -------------------------
        modelBuilder.Entity<ProductGallery>()
            .HasOne(pg => pg.Product)
            .WithMany(p => p.ProductGalleries)
            .HasForeignKey(pg => pg.ProductId)
            .IsRequired(false);

        modelBuilder.Entity<ProductReview>()
            .HasOne(pr => pr.Product)
            .WithMany(p => p.ProductReviews)
            .HasForeignKey(pr => pr.ProductId)
            .IsRequired(false);

        modelBuilder.Entity<PropertyProduct>()
            .HasOne(pp => pp.Product)
            .WithMany(p => p.PropertyProducts)
            .HasForeignKey(pp => pp.ProductId)
            .IsRequired(false);

        modelBuilder.Entity<ProductPrice>()
            .HasOne(pp => pp.Guarantee)
            .WithMany(g => g.ProductPrices)
            .HasForeignKey(pp => pp.GuaranteeId)
            .IsRequired(false);

        modelBuilder.Entity<FAQAnswer>()
            .HasOne(a => a.Question)
            .WithMany(q => q.FaqAnswers)
            .IsRequired(false); // optional


        // -------------------------
        // تغییر رفتار Delete به Restrict
        // -------------------------
        foreach (var fk in modelBuilder.Model.GetEntityTypes()
                     .SelectMany(t => t.GetForeignKeys())
                     .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade))
        {
            fk.DeleteBehavior = DeleteBehavior.Restrict;
        }

        base.OnModelCreating(modelBuilder);
    }
}
