using Ecom.Core.interfaces;
using Ecom.infrastructure.Data;
using Ecom.infrastructure.Repositries;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
        public static IServiceCollection infrastructureConfiguration(this IServiceCollection services, IConfiguration configuretion)
        {
            services.AddScoped(typeof(IGenericRepositry<>), typeof(GenericRepositry<>));

            //apply Unit of work
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            //apply DbContext
            services.AddDbContext<AppDbContext>(op =>
            {
             op.UseSqlServer(configuretion.GetConnectionString("Ecom"));

        });

            return services;
        }
    }
}
