using AutoMapper;
using Ecom.Core.Dto;
using Ecom.Core.Entites.Product;
using Ecom.Core.interfaces;
using Ecom.Core.Services;
using Ecom.infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.infrastructure.Repositries
{
    public class ProductRepositry : GenericRepositry<Product>, IProductRepositry
    {
        private readonly AppDbContext context;
        private readonly IMapper mapper;
        private readonly IImageManagementService imageManagementService;
        public ProductRepositry(AppDbContext context, IMapper mapper, IImageManagementService imageManagementService) : base(context)
        {
            this.context = context;
            this.mapper = mapper;
            this.imageManagementService = imageManagementService;
        }

        public async Task<IEnumerable<ProductDto>> GetAllAsync(string sort)
        {
            var query = context.Products
                .Include(m => m.Category)
                .Include(m => m.Photos)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(sort))
            {
                switch (sort)
                {
                    case "PricAsn":
                        query = query.OrderBy(m => m.NewPrice);
                        break;
                    case "PricDes":
                        query = query.OrderByDescending(m => m.NewPrice);
                        break;
                    default:
                        query = query.OrderBy(m => m.Name);
                        break;
                }
            }
            var result = mapper.Map<List<ProductDto>>(query);
            return result;
        }
        public async Task<bool> AddAsync(AddProductDto addProductDto)
        {
            if (addProductDto == null) return false;
            var product = mapper.Map<Product>(addProductDto);

            await context.Products.AddAsync(product);
            await context.SaveChangesAsync();

            var ImagePath = await imageManagementService.AddImageAsync(addProductDto.Photo, addProductDto.Name);

            var photo = ImagePath.Select(path => new Photo
            {
                ImageName = path,
                ProductId = product.Id,
            }).ToList();

            await context.Photos.AddRangeAsync(photo);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(UpdateProudactDto updateProudactDto)
        {
            if (updateProudactDto is null)
            {
                return false;
            }

            var FindProduct= await context.Products.Include(m=>m.Category)
                .Include(m=>m.Photos)
                .FirstOrDefaultAsync(m=>m.Id == updateProudactDto.Id);

            if (FindProduct is null)
            {
                Console.WriteLine($"Searching for product with ID: {updateProudactDto.Id}");

                
            }

            mapper.Map(updateProudactDto,FindProduct);

            var FindPhoto = await context.Photos.Where(m=>m.ProductId == updateProudactDto.Id).ToListAsync();
            
            foreach (var item in FindPhoto)
            {
                imageManagementService.DeleteImageAsync(item.ImageName);
            }

            context.Photos.RemoveRange(FindPhoto);

            var ImagePath = await imageManagementService.AddImageAsync(updateProudactDto.Photo, updateProudactDto.Name);

            var photo = ImagePath.Select(path => new Photo
            {
                ImageName = path,
                ProductId = updateProudactDto.Id,
            }).ToList();

            await context.Photos.AddRangeAsync(photo);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Product product)
        {
            var photo = await context.Photos.Where(m => m.ProductId == product.Id).ToListAsync();

            foreach(var item in photo)
            {
                imageManagementService.DeleteImageAsync($"{item.ImageName}");
            }
            context.Products.Remove(product);
            await context.SaveChangesAsync();
            return true;
        }
    }
}
