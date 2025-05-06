using Ecom.Core.Entites;
using Ecom.Core.interfaces;
using Ecom.Core.Services;
using Ecom.infrastructure.Data;
using Ecom.infrastructure.Repositries;
using Ecom.infrastructure.Repositries.Service;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System.Text;

namespace Ecom.infrastructure
{
    public static class infrastructureRegisteration
    {
        public static IServiceCollection infrastructureConfiguration(this IServiceCollection services, IConfiguration configuretion)
        {
            services.AddScoped(typeof(IGenericRepositry<>), typeof(GenericRepositry<>));

            //apply Unit of work
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            //register email sender
            services.AddScoped<IEmailService, EmailService>();
            //register IOrder Service
            services.AddScoped<IOrderService, OrderService>();

            //register token
            services.AddScoped<IGenerateToken,GenerateToken>();
            //apply Redis Connection
            services.AddSingleton<IConnectionMultiplexer>(i =>
            {
                var config = ConfigurationOptions.Parse(configuretion.GetConnectionString("redis"));
                config.AbortOnConnectFail = false; // إعادة المحاولة بعد الفشل الأول
                return ConnectionMultiplexer.Connect(config);
            });


            services.AddSingleton<IImageManagementService,ImageManagementService>();
            services.AddSingleton<IFileProvider>(new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));
            //apply DbContext
            services.AddDbContext<AppDbContext>(op =>
            {
             op.UseSqlServer(configuretion.GetConnectionString("Ecom"));

           });

            services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();


            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(x =>
            {
                x.Cookie.Name = "token";
                x.Events.OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return Task.CompletedTask;
                };
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,

                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuretion["Token:Secret"])),
                    ValidateIssuer = true,
                    ValidIssuer = configuretion["Token:Issuer"],
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };
                x.Events = new JwtBearerEvents
                {

                    OnMessageReceived = context =>
                    {
                        var token = context.Request.Cookies["token"];
                        context.Token = token;
                        return Task.CompletedTask;
                    }
                };
            });

            return services;
        }
    }
}
