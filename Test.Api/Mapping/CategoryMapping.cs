using AutoMapper;
using Test.Core.DTOS;
using Test.Core.Entities.Product;

namespace Test.Api.Mapping
{
    public class CategoryMapping : Profile
    {
        public CategoryMapping()
        {
            CreateMap<Category, CategoryDTO>()
                .ForMember(cdto => cdto.ProductCount, opt => opt.MapFrom(c => c.Products.Count()))
                .ReverseMap();

            CreateMap<Category, CreateCategoryDTO>().ReverseMap();
        }
    }
}
