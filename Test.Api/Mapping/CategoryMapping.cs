using AutoMapper;
using Ecom.Core.DTOS;
using Ecom.Core.Entities.Product;

namespace Ecom.Api.Mapping
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
