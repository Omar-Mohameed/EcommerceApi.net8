using AutoMapper;
using ECommerce.Core.DTOS;
using ECommerce.Core.Entities.Product;

namespace ECommerce.Api.Mapping
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
