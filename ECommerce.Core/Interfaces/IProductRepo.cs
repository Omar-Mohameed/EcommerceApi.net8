using ECommerce.Core.DTOS;
using ECommerce.Core.Entities.Product;
using ECommerce.Core.Shared;
using ErrorOr;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.Interfaces
{
    public interface IProductRepo : IGenericRepo<Product>
    {
        Task<ErrorOr<Product>> AddProductAsync(Product product, IFormFileCollection photos);
        Task<ErrorOr<Product>> UpdateProductAsync(Product product, IFormFileCollection photos);
        Task<ErrorOr<bool>> DeleteProductAsync(Product product);
        Task<ErrorOr<PagedResult<ProductDTO>>> GetAllProductAsync(ProductParams productParams);

    }
}
