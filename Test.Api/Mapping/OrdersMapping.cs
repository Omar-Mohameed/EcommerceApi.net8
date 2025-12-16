using AutoMapper;
using Test.Core.DTOS.Orders;
using Test.Core.Entities;
using Test.Core.Entities.Order;

namespace Test.Api.Mapping
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
