using Ecom.Core.interfaces;
using Ecom.infrastructure.Repositries;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.infrastructure
{
    public static class infrastructureRegisteration
    {
        public static IServiceCollection infrastructureConfiguration(this IServiceCollection services)
        {
            services.AddScoped(typeof(IGenericRepositry<>), typeof(GenericRepositry<>));
            services.AddScoped<ICategoryRepositry,CategoryRepositry>();
            services.AddScoped<IProductRepositry,ProductRepositry>();
            services.AddScoped<IPhotoRepositry,PhotoRepositry>();
            return services;
        }
    }
}
