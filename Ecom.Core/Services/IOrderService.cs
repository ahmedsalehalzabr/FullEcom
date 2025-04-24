using Ecom.Core.Dto;
using Ecom.Core.Entites.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.Services
{
    public interface IOrderService
    {
        Task<Orders> CreateOrdersAsync(OrderDto orderDto,string BuyerEmail);
        Task<IReadOnlyList<Orders>> GetAllOrdersForUserAsync(string BuyerEmail);
        Task<Orders> GetOrderByIdAsync(int Id, string BuyerEmail);
        Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodAsync();
    }
}
