using AutoMapper;
using ECommerce.Api.Helper;
using ECommerce.Core.DTOS;
using ECommerce.Core.Entities.Product;
using ECommerce.Core.Interfaces;
using ErrorOr;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ECommerce.Api.Controllers
{
    public class ProductsController : BaseController
    {
        public ProductsController(IUnitOfWork work, IMapper mapper) : base(work, mapper)
        {
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(AddProductDTO addProductDTO)
        {
            var photos = addProductDTO.Photos;
            var product = mapper.Map<Product>(addProductDTO);
            var result = await work.Products.AddProductAsync(product, photos);
            if (result.IsError)
            {
                return Problem(statusCode: 400, title: result.FirstError.Code, detail: result.FirstError.Description);
            }
            await work.SaveChangesAsync();
            var getResult = await work.Products.GetByIdAsync(product.Id, p => p.Category, p => p.Photos);
            var productafterinsert = mapper.Map<ProductDTO>(getResult.Value);
            return Ok(new ResponseApi(200, "Product added successfully", productafterinsert));

        }


        [HttpGet]
        public async Task<IActionResult> GetAllProduct([FromQuery]ProductParams p)
        {
            var result =await work.Products.GetAllProductAsync(p);
            return result.Match(
                products=>
                {
                    return Ok(new ResponseApi(200, data: result.Value));
                },
                errors=>
                {
                    return Problem(
                        statusCode: 400,
                        title: result.FirstError.Code,
                        detail: result.FirstError.Description
                    );
                }
            );
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await work.Products.GetByIdAsync(id, p => p.Category, p => p.Photos);
            return result.Match(
                product =>
                {
                    var ProductResult = mapper.Map<ProductDTO>(product);
                    return Ok(new ResponseApi(200, data: ProductResult));
                },
                errors =>
                {
                    return Problem(
                        statusCode: result.FirstError.Type == ErrorType.NotFound ? 404: 400,
                        title: result.FirstError.Code,
                        detail: result.FirstError.Description
                    );
                }
            );
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, AddProductDTO product)
        {
            var result =await work.Products.GetByIdAsync(id);
            if (result.IsError)
            {
                return Problem(
                    statusCode: result.FirstError.Type == ErrorType.NotFound ? 404 : 400,
                    title: result.FirstError.Code,
                    detail: result.FirstError.Description);
            }
            result.Value.Name = product.Name;
            result.Value.Description = product.Description;
            result.Value.NewPrice = product.NewPrice;
            result.Value.OldPrice = product.OldPrice;
            result.Value.CategoryId = product.CategoryId;

            var res = await work.Products.UpdateProductAsync(result.Value,product.Photos);
            if (res.IsError)
            {
                return Problem(
                    statusCode: result.FirstError.Type == ErrorType.NotFound ? 404 : 400,
                    title: result.FirstError.Code,
                    detail: result.FirstError.Description);
            }
            
            await work.Products.UpdateAsync(result.Value);
            await work.SaveChangesAsync();
            return Ok(new ResponseApi(200, "Product updated successfully."));
        }
        [HttpDelete("{id}")] 
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var result = await work.Products.GetByIdAsync(id);
            if (result.IsError)
            {
                return Problem(
                    statusCode: result.FirstError.Type == ErrorType.NotFound ? 404 : 400,
                    title: result.FirstError.Code,
                    detail: result.FirstError.Description);
            }

            var ResultDeleted = await work.Products.DeleteProductAsync(result.Value);

            if (ResultDeleted.IsError)
            {
                return Problem(
                    statusCode: ResultDeleted.FirstError.Type==ErrorType.NotFound?404:400,
                    title:ResultDeleted.FirstError.Code,
                    detail: ResultDeleted.FirstError.Description
                );
            }
            await work.SaveChangesAsync();
            return Ok(new ResponseApi(200, "Product removed successfully"));
        }
    }
}
