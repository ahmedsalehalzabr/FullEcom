using AutoMapper;
using Ecom.Core.Dto;
using Ecom.Core.Entites;
using Ecom.Core.Entites.Order;

namespace Ecom.Api.Mapping
{
    public class OrderMapping:Profile
    {
        public OrderMapping()
        {
            CreateMap<Orders, OrderToReturnDto>()
                .ForMember(m=>m.deliveryMethod,o=>o.MapFrom(s=>s.deliveryMethod.Name)).ReverseMap();
            CreateMap<OrderItem, OrderItemDto>().ReverseMap();
            CreateMap<ShippingAddress, ShipAddressDto>().ReverseMap();
            CreateMap<Address, ShipAddressDto>().ReverseMap();
        }
    }
}
