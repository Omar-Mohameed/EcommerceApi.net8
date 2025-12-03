using AutoMapper;
using ErrorOr;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Core.DTOS.Orders;
using Test.Core.Entities.Order;
using Test.Core.Interfaces;
using Test.Core.Services;
using Test.Infrastructure.Data;

namespace Test.Infrastructure.Repositores.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public OrderService(IUnitOfWork unitOfWork, AppDbContext appDbContext, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _context = appDbContext;
            _mapper = mapper;
        }

        public async Task<ErrorOr<Orders>> CreateOrdersAsync(OrderDTO orderDTO, string BuyerEmail)
        {
            // Get basket from redis
            var basket = await _unitOfWork.Baskets.GetBasketAsync(orderDTO.basketId);
            List<OrderItem> OrderItems = new List<OrderItem>();
            foreach (var item in basket.basketItems)
            {
                var ProductResult = await _unitOfWork.Products.GetByIdAsync(item.Id);
                if (ProductResult.IsError)
                {
                    var error = ProductResult.FirstError;
                    return Error.NotFound(
                        code: $"Product.{error.Type}",
                        description: error.Description
                    );
                }
                var Product = ProductResult.Value;
                var OrderItem = new OrderItem
                    (Product.Id, item.Image, Product.Name, Product.NewPrice, item.Qunatity);
                OrderItems.Add(OrderItem);
            }
            var deliverMethod = await _context.DeliveryMethods.FirstOrDefaultAsync(m => m.Id == orderDTO.deliveryMethodId);

            var subTotal = OrderItems.Sum(m => m.Price * m.Quntity);

            var ship = _mapper.Map<ShippingAddress>(orderDTO.shipAddress);

            var Order = new Orders(BuyerEmail, subTotal, OrderItems,ship, deliverMethod);

            await _context.Orders.AddAsync(Order);
            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.Baskets.DeleteBasketAsync(orderDTO.basketId);
            return Order;
        }

        public async Task<IReadOnlyList<OrderReturnDto>> GetAllOrdersForUserAsync(string BuyerEmail)
        {
            var orders = await _context.Orders.Where(o=>o.BuyerEmail == BuyerEmail)
                .Include(o=>o.OrderItems).Include(o=>o.DeliveryMethod).ToListAsync();

            var res = _mapper.Map<IReadOnlyList<OrderReturnDto>>(orders);
            return res;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodAsync()
        {
            return await _context.DeliveryMethods.AsNoTracking().ToListAsync();
        }

        public async Task<OrderReturnDto> GetOrderByIdAsync(int Id, string BuyerEmail)
        {
            var order = await _context.Orders.Where(o=>o.Id == Id && o.BuyerEmail == BuyerEmail)
                .Include(o=>o.OrderItems).Include(o=>o.DeliveryMethod)
                .FirstOrDefaultAsync();

            var res = _mapper.Map<OrderReturnDto>(order);
            return res;
        }
    }
}
