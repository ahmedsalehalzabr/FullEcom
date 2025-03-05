using AutoMapper;
using Ecom.Core.Dto;
using Ecom.Core.Entites.Product;
namespace Ecom.Api.Mapping
{
    public class CategoryMapping:Profile
    {
        public CategoryMapping()
        {
            CreateMap<CategoryDto,Category>().ReverseMap();
            CreateMap<UbdataCategoryDto, Category>().ReverseMap();

        }
    }
}
