using Ecom.Core.interfaces;
using Ecom.Core.Services;
using Ecom.infrastructure.Data;
using Ecom.infrastructure.Repositries;
using Ecom.infrastructure.Repositries.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using StackExchange.Redis;

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

            return services;
        }
    }
}
