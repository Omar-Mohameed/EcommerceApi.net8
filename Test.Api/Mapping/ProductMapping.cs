using AutoMapper;
using Test.Core.DTOS;
using Test.Core.Entities.Product;

namespace Test.Api.Mapping
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
