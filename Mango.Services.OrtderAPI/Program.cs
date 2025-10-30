
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Mango.ServiceBus;
using Mango.services.OrtderAPI.Data;
using Mango.services.ShoppingCartAPI;
using Mango.Services.OrtderAPI.Services.IServices;
using Mango.Services.OrtderAPI.Services;
using Mango.Services.OrtderAPI.Utility;
using Mango.services.OrtderAPI.Extensions;

namespace Mango.services.ProdcutAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<ApplicationDBContext>(options =>
            {
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();
            builder.Services.AddSingleton(mapper);
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()); //for using mapper as DI
            builder.Services.AddScoped<IProductService,ProductService>();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<BackendAuthenticationHttpClientHandler>();
           
            builder.Services.AddScoped<IMessageBus, MessageBus>();
            builder.Services.AddHttpClient("Product", 
                u => u.BaseAddress = new Uri(builder.Configuration["ServiceUrls:ProductAPI"])).AddHttpMessageHandler<BackendAuthenticationHttpClientHandler>();
          
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(option =>
            {
                option.AddSecurityDefinition(name: JwtBearerDefaults.AuthenticationScheme, securityScheme: new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "Enter the Bearer Authentication string as following: `Bearer Generated-JWT-Token`",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = JwtBearerDefaults.AuthenticationScheme
                            }
                        },new string[] { }

                    }
                });

            });

            builder.AddAppAuthentication(); //extension method for authentication
            builder.Services.AddAuthorization();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            // ApplyMigration();
            app.Run();

            //if there are any pending migrations it automatically apply by below process

            void ApplyMigration()
            {
                using (var scope = app.Services.CreateScope())   //it will get all the services
                {
                    var _db = scope.ServiceProvider.GetRequiredService<ApplicationDBContext>();

                    if (_db.Database.GetPendingMigrations().Count() > 0)
                    {
                        _db.Database.Migrate();
                    }
                }
            }
        }
    }
}
