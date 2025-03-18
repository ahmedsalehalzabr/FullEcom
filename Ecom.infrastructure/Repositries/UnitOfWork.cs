using AutoMapper;
using Ecom.Core.interfaces;
using Ecom.Core.Services;
using Ecom.infrastructure.Data;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.infrastructure.Repositries
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly IMapper mapper;
        private readonly IImageManagementService imageManagementService;
        private readonly IConnectionMultiplexer redis;

        public ICategoryRepositry CategoryRepositry { get; }

        public IPhotoRepositry PhotoRepositry { get; }

        public IProductRepositry ProductRepositry { get; }

        public ICustomerBasketRepositry CustomerBasketRepositry { get; }

        public UnitOfWork(AppDbContext context, IMapper mapper, IImageManagementService imageManagementService,IConnectionMultiplexer redis)
        {
            _context = context;
            this.mapper = mapper;
            this.imageManagementService = imageManagementService;
            this.redis = redis;

            CategoryRepositry = new CategoryRepositry(_context);
            PhotoRepositry = new PhotoRepositry(_context);
            ProductRepositry = new ProductRepositry(_context,mapper,imageManagementService);
            CustomerBasketRepositry = new CustomerBasketRepositry(redis);
        }
    }
}
