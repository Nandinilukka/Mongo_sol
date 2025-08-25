using Microsoft.AspNetCore.Authentication.Cookies;
using Mongo.Web.Service;
using Mongo.Web.Service.IService;
using Mongo.Web.Utility;

namespace Mongo.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddHttpContextAccessor(); //for using session
            builder.Services.AddHttpClient(); //for using httpclient

            builder.Services.AddHttpClient<IProductService, ProductService>();
            builder.Services.AddHttpClient<ICouponService, CouponService>();
            builder.Services.AddHttpClient<ICartService, CartService>();
            builder.Services.AddHttpClient<IAuthService, AuthService>();

            SD.CouponAPIBase = builder.Configuration["ServiceUrls:CouponAPI"]; //get the base url from appsettings.json
            SD.AuthAPIBase = builder.Configuration["ServiceUrls:AuthAPI"]; //get the base url from appsettings.json
            SD.ProductAPIBase = builder.Configuration["ServiceUrls:ProductAPI"]; //get the base url from appsettings.json
            SD.ShoppingCartAPIBase = builder.Configuration["ServiceUrls:ShoppingCartAPI"]; //get the base url from appsettings.json

            builder.Services.AddScoped<ITokenProvider, TokenProvider>(); //for using token provider
            builder.Services.AddScoped<IBaseService, BaseService>(); //for using base service
            builder.Services.AddScoped<IProductService, ProductService>(); //for using product service
            builder.Services.AddScoped<IAuthService, AuthService>(); //for using auth service
            builder.Services.AddScoped<ICouponService, CouponService>(); //for using coupon service
            builder.Services.AddScoped<ICartService, CartService>(); //for using cart service

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
                    options.LoginPath = "/Auth/Login"; //redirect to login page if not authenticated
                    options.AccessDeniedPath = "/Auth/AccessDenied"; //redirect to access denied page if not authorized
                });


            // Add CORS policy
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                builder =>
                {
                    builder.AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                });
            });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
