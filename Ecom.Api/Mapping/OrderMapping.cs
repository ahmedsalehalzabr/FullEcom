using AutoMapper;
using Ecom.Core.Dto;
using Ecom.Core.Entites.Order;

namespace Ecom.Api.Mapping
{
    public class OrderMapping:Profile
    {
        public OrderMapping()
        {
            CreateMap<ShippingAddress, ShipAddressDto>().ReverseMap();
        }
    }
}
