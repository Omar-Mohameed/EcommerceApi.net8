using AutoMapper;
using Ecom.Core.DTOS.Orders;
using Ecom.Core.Entities;
using Ecom.Core.Entities.Order;

namespace Ecom.Api.Mapping
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
