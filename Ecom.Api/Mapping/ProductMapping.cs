﻿using AutoMapper;
using Ecom.Core.Dto;
using Ecom.Core.Entites.Product;

namespace Ecom.Api.Mapping
{
    public class ProductMapping:Profile
    {
        public ProductMapping()
        {
            CreateMap<Product, ProductDto>
                ().ForMember(x => x.CategoryName,
                op => op.MapFrom(scr => scr.Category.Name)).ReverseMap();

            CreateMap<Photo, PhotoDto> ().ReverseMap();

            CreateMap<AddProductDto,Product>()
                .ForMember(m=>m.Photos,op=>op.Ignore())
                .ReverseMap();
            CreateMap<UpdateProudactDto, Product>()
               .ForMember(m => m.Photos, op => op.Ignore())
               .ReverseMap();
        }
    }
}
