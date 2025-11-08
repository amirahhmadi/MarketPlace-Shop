using GameOnline.Core.Services.AccountService.Queries;
using GameOnline.DataBase.Context;
using GameOnline.Core.Services.AddressService.Queries;
using Microsoft.EntityFrameworkCore;
using GameOnline.Core.Services.BrandServices.Commands;
using GameOnline.Core.Services.BrandServices.Queries;
using GameOnline.Core.Services.CartService.Commands;
using GameOnline.Core.Services.CartService.Queries;
using GameOnline.Core.Services.CategoryServices.Commands;
using GameOnline.Core.Services.CategoryServices.Queries;
using GameOnline.Core.Services.ColorServices.Commands;
using GameOnline.Core.Services.ColorServices.Queries;
using GameOnline.Core.Services.Comment_FAQ.Queries.FAQ;
using GameOnline.Core.Services.DiscountServices.Commands;
using GameOnline.Core.Services.DiscountServices.Queries;
using GameOnline.Core.Services.GalleryServices.Commands;
using GameOnline.Core.Services.GuaranteeServices.Commands;
using GameOnline.Core.Services.ProductServices.Commands;
using GameOnline.Core.Services.GalleryServices.Queries;
using GameOnline.Core.Services.GuaranteeServices.Queries;
using GameOnline.Core.Services.ProductServices.Queries;
using GameOnline.Core.Services.PropertyService.Commands.PropertyGroup;
using GameOnline.Core.Services.PropertyService.Commands.PropertyName;
using GameOnline.Core.Services.PropertyService.Commands.PropertyValue;
using GameOnline.Core.Services.PropertyService.Queries.PropertyGroup;
using GameOnline.Core.Services.PropertyService.Queries.PropertyName;
using GameOnline.Core.Services.PropertyService.Queries.PropertyValue;
using GameOnline.Core.Services.RoleService.Queries;
using GameOnline.Core.Services.SliderServices.Queries;
using GameOnline.Core.Services.RoleService.Commands;
using GameOnline.Core.Services.SliderServices.Commands;
using GameOnline.Core.Services.UserService.Commands;
using GameOnline.Core.Services.UserService.Queries;
using GameOnline.Core.Services.AccountService.Commands;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<GameOnlineContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

#region Commands
builder.Services.AddHttpClient();

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<IBrandServiceCommand, BrandServiceCommand>();
builder.Services.AddTransient<ICartServiceCommand, CartServiceCommand>();
builder.Services.AddTransient<ICategoryServicesCommand, CategoryServicesCommand>();
builder.Services.AddTransient<IColorServicesCommand, ColorServicesCommand>();
builder.Services.AddTransient<IDiscountServicesCommand, DiscountServicesCommand>();
builder.Services.AddTransient<IGalleryServicesCommand, GalleryServicesCommand>();
builder.Services.AddTransient<IGuaranteeServiceCommand, GuaranteeServiceCommand>();
builder.Services.AddTransient<IProductServicesCommand, ProductServicesCommand>();
builder.Services.AddTransient<IPropertyGroupCommand, PropertyGroupCommand>();
builder.Services.AddTransient<IPropertyNameCommand, PropertyNameCommand>();
builder.Services.AddTransient<IPropertyValueCommand, PropertyValueCommand>();
builder.Services.AddTransient<IRoleServiceCommand, RoleServiceCommand>();
builder.Services.AddTransient<ISliderServiceCommand, SliderServiceCommand>();
builder.Services.AddTransient<IUserServiceCommand, UserServiceCommand>();
builder.Services.AddTransient<IAccountServiceCommand, AccountServiceCommand>();

#endregion

#region Queries

builder.Services.AddTransient<IAddressServiceQuery, AddressServiceQuery>();
builder.Services.AddTransient<IBrandServiceQuery, BrandServiceQuery>();
builder.Services.AddTransient<ICartServiceQuery, CartServiceQuery>();
builder.Services.AddTransient<ICategoryServicesQuery, CategoryServicesQuery>();
builder.Services.AddTransient<IColorServicesQuery, ColorServicesQuery>();
builder.Services.AddTransient<IFaqServiceQuery, FaqServiceQuery>();
builder.Services.AddTransient<IDiscountServicesQuery, DiscountServicesQuery>();
builder.Services.AddTransient<IGalleryServicesQuery, GalleryServicesQuery>();
builder.Services.AddTransient<IGuaranteeServiceQuery, GuaranteeServiceQuery>();
builder.Services.AddTransient<IProductServicesQuery, ProductServicesQuery>();
builder.Services.AddTransient<IPropertyGroupQuery, PropertyGroupQuery>();
builder.Services.AddTransient<IPropertyNameQuery, PropertyNameQuery>();
builder.Services.AddTransient<IPropertyValueQuery, PropertyValueQuery>();
builder.Services.AddTransient<IRoleServiceQuery, RoleServiceQuery>();
builder.Services.AddTransient<ISliderServiceQuery, SliderServiceQuery>();
builder.Services.AddTransient<IUserServiceQuery, UserServiceQuery>();
builder.Services.AddTransient<IAccountServiceQuery, AccountServiceQuery>();

#endregion


builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = "GameOnline-Login";
        options.DefaultSignInScheme = "GameOnline-Login";
        options.DefaultChallengeScheme = "GameOnline-Login";
    })
    .AddCookie("GameOnline-Login", options =>
    {
        options.LoginPath = "/Login";
        options.LogoutPath = "/Logout";
        options.AccessDeniedPath = "/AccessDenied";

        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // روی HTTPS
        options.Cookie.SameSite = SameSiteMode.Strict;
        options.SlidingExpiration = true;
        options.ExpireTimeSpan = TimeSpan.FromDays(30);
    });

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "area",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "Default",
    pattern: "{controller=home}/{action=Index}/{id?}");

app.Run();
