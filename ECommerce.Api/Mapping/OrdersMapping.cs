using AutoMapper;
using ECommerce.Core.DTOS.Orders;
using ECommerce.Core.Entities;
using ECommerce.Core.Entities.Order;

namespace ECommerce.Api.Mapping
{
    public class OrdersMapping : Profile
    {
        public OrdersMapping()
        {
            CreateMap<Orders, OrderReturnDto>()
                .ForMember(dto=>dto.DeliveryMethod,
                opt=>opt.MapFrom(o=>o.DeliveryMethod.Name)).ReverseMap();

            CreateMap<OrderItem, OrderItemDto>().ReverseMap();

            CreateMap<ShippingAddress, ShipAddressDTO>().ReverseMap();

            CreateMap<Address,ShipAddressDTO>().ReverseMap();

        }
    }
}
