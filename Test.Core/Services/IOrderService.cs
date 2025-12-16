using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecom.Core.DTOS.Orders;
using Ecom.Core.Entities.Order;

namespace Ecom.Core.Services
{
    public interface IOrderService
    {
        Task<ErrorOr<Orders>> CreateOrdersAsync(OrderDto orderDTO, string BuyerEmail);
        Task<IReadOnlyList<OrderReturnDto>> GetAllOrdersForUserAsync(string BuyerEmail);
        Task<OrderReturnDto> GetOrderByIdAsync(int Id, string BuyerEmail);
        Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodAsync();
    }
}
