using AutoMapper;
using Ecom.Core.Dto;
using Ecom.Core.Entites.Product;
using Ecom.Core.interfaces;
using Ecom.Core.Services;
using Ecom.Core.Sharing;
using Ecom.infrastructure.Data;
using Microsoft.EntityFrameworkCore;
 
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

        public async Task<ReturnProductDto> GetAllAsync(ProductParams productParams)
        {
            var query = context.Products
                .Include(m => m.Category)
                .Include(m => m.Photos)
                .AsNoTracking();

            //filtering by word
            if (!string.IsNullOrEmpty(productParams.Search))
            {
                var searchWord = productParams.Search.Split(' ');
                query = query.Where(m => searchWord.All(word =>
                m.Name.ToLower().Contains(word.ToLower()) || 
                m.Description.ToLower().Contains(word.ToLower())
                ));
            }
                //query = query.Where(m => m.Name.ToLower().Contains(productParams.Search.ToLower())
                //|| m.Description.ToLower().Contains(productParams.Search.ToLower()));

            //filtering by category id
            if (productParams.CategoryId.HasValue)
                query = query.Where(m => m.CategoryId == productParams.CategoryId);
            

            if (!string.IsNullOrEmpty(productParams.Sort))
            {
                query = productParams.Sort switch
                {
                    "PricAcn" => query.OrderBy(m => m.NewPrice),
                    "PricDce" => query.OrderByDescending(m => m.NewPrice),
                    _ => query.OrderBy(m => m.Name),
                };
            }

            ReturnProductDto returnProductDto = new ReturnProductDto();
            returnProductDto.TotalCount = query.Count();

            query = query.Skip((productParams.pageSize) *(productParams.PageNumber - 1)).Take(productParams.pageSize);

            returnProductDto.products = mapper.Map<List<ProductDto>>(query);
            return returnProductDto;
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
