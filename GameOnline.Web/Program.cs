using GameOnline.DataBase.Context;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.EntityFrameworkCore;
using GameOnline.Core.Services.BrandServices.BrandServicesAdmin;
using GameOnline.Core.Services.GuaranteeServices.GuaranteeServicesAdmin;
using GameOnline.Core.Services.SliderServices.SliderServicesAdmin;
using GameOnline.Core.Services.CategoryServices.CategoryServicesAdmin;
using GameOnline.Core.Services.ColorServices.ColorServicesAdmin;
using GameOnline.Core.Services.ProductServices.ProductServicesAdmin;
using GameOnline.Core.Services.GalleryServices.GalleryServicesAdmin;
using GameOnline.Core.Services.PropertyService.PropertyGroupService;
using GameOnline.Core.Services.PropertyService.PropertyNameService;
using GameOnline.Core.Services.PropertyService.PropertyValueService;
using GameOnline.Core.Services.DiscountServices.DiscountServicesAdmin;
using GameOnline.Core.Services.SliderServices.SliderServicesClient;
using GameOnline.Core.Services.ProductServices.ProductServicesClient;
using GameOnline.Core.Services.Comment_FAQ.Client;
using GameOnline.Core.Services.UserService.UserServiceAdmin;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<GameOnlineContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

#region Admin

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<IBrandServiceAdmin, BrandServiceAdmin>();
builder.Services.AddTransient<IGuaranteeServiceAdmin, GuaranteeServiceAdmin>();
builder.Services.AddTransient<ISliderServiceAdmin, SliderServiceAdmin>();
builder.Services.AddTransient<ICategoryServiceAdmin, CategoryServiceAdmin>();
builder.Services.AddTransient<IColorServicesAdmin, ColorServicesAdmin>();
builder.Services.AddTransient<IProductServicesAdmin, ProductServicesAdmin>();
builder.Services.AddTransient<IGalleryServicesAdmin, GalleryServicesAdmin>();
builder.Services.AddTransient<IPropertyGroupServiceAdmin, PropertyGroupServiceAdmin>();
builder.Services.AddTransient<IPropertyNameServiceAdmin, PropertyNameServiceAdmin>();
builder.Services.AddTransient<IPropertyValueServiceAdmin, PropertyValueServiceAdmin>();
builder.Services.AddTransient<IDiscountServicesAdmin, DiscountServicesAdmin>();
builder.Services.AddTransient<IUserServiceAdmin, UserServiceAdmin>();
builder.Services.AddTransient<IAccountServiceAdmin, AccountServiceAdmin>();

#endregion

#region Client

builder.Services.AddTransient<ISliderServiceClient, SliderServiceClient>();
builder.Services.AddTransient<IProductServicesClient, ProductServicesClient>();
builder.Services.AddTransient<IFaqServiceClient, FaqServiceClient>();

#endregion

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "area",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "Default",
    pattern: "{controller=home}/{action=Index}/{id?}");

app.Run();
