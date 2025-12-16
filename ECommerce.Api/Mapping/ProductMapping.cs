using AutoMapper;
using ECommerce.Core.DTOS;
using ECommerce.Core.Entities.Product;

namespace ECommerce.Api.Mapping
{
    public class ProductMapping: Profile
    {
        public ProductMapping()
        {
            CreateMap<Product, ProductDTO>().ForMember(dest => dest.CategoryName,
                opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.Photos, opt => opt.MapFrom(src => src.Photos))
                .ReverseMap();

            CreateMap<Photo, PhotoDTO>().ReverseMap();

            CreateMap<AddProductDTO, Product>()
            .ForMember(m => m.Photos, op => op.Ignore())
            .ReverseMap();

        }
    }
}
