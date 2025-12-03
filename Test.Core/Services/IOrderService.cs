using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Core.DTOS.Orders;
using Test.Core.Entities.Order;

namespace Test.Core.Services
{
    public interface IOrderService
    {
        Task<ErrorOr<Orders>> CreateOrdersAsync(OrderDTO orderDTO, string BuyerEmail);
        Task<IReadOnlyList<OrderReturnDto>> GetAllOrdersForUserAsync(string BuyerEmail);
        Task<OrderReturnDto> GetOrderByIdAsync(int Id, string BuyerEmail);
        Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodAsync();
    }
}
