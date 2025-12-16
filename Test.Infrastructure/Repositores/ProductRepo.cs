using AutoMapper;
using ErrorOr;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Core.DTOS;
using Test.Core.Entities.Product;
using Test.Core.Interfaces;
using Test.Core.Services;
using Test.Core.Services.Errors;
using Test.Core.Shared;
using Test.Infrastructure.Data;
using Test.Infrastructure.Repositores.Services;

namespace Test.Infrastructure.Repositores
{
    public class ProductRepo : GenericRepo<Product>, IProductRepo
    {
        private readonly AppDbContext context;
        private readonly IMapper mapper;
        private readonly IImageManagementService imageManagementService;

        public ProductRepo(AppDbContext context, IMapper mapper, IImageManagementService imageManagementService) : base(context)
        {
            this.context = context;
            this.mapper = mapper;
            this.imageManagementService = imageManagementService;
        }

        public async Task<ErrorOr<PagedResult<ProductDTO>>> GetAllProductAsync(ProductParams productParams)
        {
            IQueryable<Product> query = context.Products
                .Include(p => p.Photos)
                .Include(p => p.Category).AsNoTracking();
            // search
            //filtering by word
            if (!string.IsNullOrEmpty(productParams.Search))
            {
                var searchWords = productParams.Search.Split(' ');
                query = query.Where(m => searchWords.Any(word =>

                    m.Name.ToLower().Contains(word.ToLower()) ||
                    m.Description.ToLower().Contains(word.ToLower())

                ));
            }


            // filter by category
            if (productParams.CategoryId.HasValue)
                query = query.Where(x => x.CategoryId == productParams.CategoryId);
            // Sorting
            query = productParams.Sort?.ToLower() switch
            {
                "priceasc" => query.OrderBy(x => x.NewPrice),
                "pricedesc" => query.OrderByDescending(x => x.NewPrice),
                "nameasc" => query.OrderBy(x => x.Name),
                "namedesc" => query.OrderByDescending(x => x.Name),
                _ => query.OrderBy(x => x.Id)
            };
            // Total Count before pagination
            var totalCount = await query.CountAsync();

            // Pagination
            var products = await query
                .Skip((productParams.PageNumber - 1) * productParams.PageSize)
                .Take(productParams.PageSize)
                .ToListAsync();
            // Mapping
            var data = mapper.Map<List<ProductDTO>>(products);

            // Return PagedResult
            return new PagedResult<ProductDTO>
            {
                PageNumber = productParams.PageNumber,
                PageSize = productParams.PageSize,
                TotalCount = totalCount,
                Data = data
            };
        }

        public async Task<ErrorOr<Product>> AddProductAsync(Product product, IFormFileCollection photos)
        {
            try
            {
                await context.Products.AddAsync(product);
                await context.SaveChangesAsync();
                // 2) Validate photos
                if (photos == null || photos.Count == 0)
                {
                    return ProductErrors.InvalidPhoto;
                }
                if (string.IsNullOrWhiteSpace(product.Name) ||
                    product.Name.Length < 3 ||
                    product.Name.Length > 50)
                {
                    return ProductErrors.InvalidName;
                }

                var savedPaths = await imageManagementService.AddImageAsync(photos, product.Name);

                var photoEntities = savedPaths.Select(path => new Photo
                {
                    ImageName = path,
                    ProductId = product.Id,
                }).ToList();

                await context.Photos.AddRangeAsync(photoEntities);

                return product;
            }
            catch (Exception ex)
            {
                return Error.Failure(
                    code: "Product.AddFailed",
                    description: ex.Message
                );
            }
        }

        public async Task<ErrorOr<Product>> UpdateProductAsync(Product product, IFormFileCollection photos)
        {
            var FindPhoto = await context.Photos.Where(m => m.ProductId == product.Id).ToListAsync();
            foreach (var item in FindPhoto)
            {
                imageManagementService.DeleteImageAsync(item.ImageName);
            }
            context.Photos.RemoveRange(FindPhoto);

            var ImagePath = await imageManagementService.AddImageAsync(photos, product.Name);

            var photo = ImagePath.Select(path => new Photo
            {
                ImageName = path,
                ProductId = product.Id,
            }).ToList();

            await context.Photos.AddRangeAsync(photo);

            await context.SaveChangesAsync();
            //return true;
            return product;

        }
        
        public async Task<ErrorOr<bool>> DeleteProductAsync(Product product)
        {
            var photo = await context.Photos.Where(m => m.ProductId == product.Id)
            .ToListAsync();
            foreach (var item in photo)
            {
                imageManagementService.DeleteImageAsync(item.ImageName);
            }
            context.Products.Remove(product);
            return true;
        }
    
    }
}
