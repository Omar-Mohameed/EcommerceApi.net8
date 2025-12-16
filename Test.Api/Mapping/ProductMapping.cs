using AutoMapper;
using Ecom.Core.DTOS;
using Ecom.Core.Entities.Product;

namespace Ecom.Api.Mapping
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
