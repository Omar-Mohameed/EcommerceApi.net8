using AutoMapper;
using Azure.Core;
using ErrorOr;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Test.Api.Helper;
using Test.Core.DTOS;
using Test.Core.Entities.Product;
using Test.Core.Interfaces;

namespace Test.Api.Controllers
{
    public class CategoriesController(IUnitOfWork work, IMapper mapper) : BaseController(work, mapper)
    {

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var cats = await work.Categories.GetAllAsync(c=>c.Products);
            var catsDto = mapper.Map<List<CategoryDTO>>(cats.Value);
            return Ok(new ResponseApi(200,data: catsDto));
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var result = await work.Categories.GetByIdAsync(id, c=>c.Products);
            //var cat = result.Value;
            //var catDto = mapper.Map<CategoryDTO>(cat);
            //return Ok(new ResponseApi(200, data: catDto));

            return result.Match(
            category =>
            {
                // Success → map to DTO لو حابب
                var dto = _mapper.Map<CategoryDTO>(category);
                return Ok(new ResponseApi(200, "Category fetched successfully", dto));
            },
            errors =>
            {
                // Failure → نجيب أول Error ونرجعه
                var firstError = errors.First();
                return Problem(
                    statusCode: firstError.Type == ErrorType.NotFound ? 404 : 400,
                    title: firstError.Code,
                    detail: firstError.Description
                );
            }
            );
        }
        [HttpPost]
        public async Task<IActionResult> AddCategory(CreateCategoryDTO categoryDto)
        {
            var cat = mapper.Map<Category>(categoryDto);
            var result = await work.Categories.AddAsync(cat);

            if (result.IsError)
            {
                var firstError = result.FirstError;
                return Problem(
                    statusCode: 400,
                    title: firstError.Code,
                    detail: firstError.Description
                );
            }

            // Success → Save changes
            await work.SaveChangesAsync();
            var category = mapper.Map<CategoryDTO>(result.Value);
            return Ok(new ResponseApi(200, "Category Created Successfully", category));
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, CreateCategoryDTO createCategoryDTO)
        {
            var result = await work.Categories.GetByIdAsync(id,c=>c.Products);

            if (result.IsError)
            {
                var firstError = result.FirstError;
                return Problem(
                    statusCode:firstError.Type == ErrorType.NotFound? 404 : 400,
                    title: firstError.Code,
                    detail: firstError.Description
                    );
            }
            var category = result.Value;
            // check DuplicateCategoryName
            if(await work.Categories.DuplicateName(createCategoryDTO.Name))
            {
                return Problem(
                    statusCode: 409,
                    title: "Category.DuplicateName",
                    detail: $"Category with name '{category.Name}' already exists."
                );
            }
            category.Name = createCategoryDTO.Name;
            category.Description = createCategoryDTO.Description;

            var updatedResult = await work.Categories.UpdateAsync(result.Value);
            if (updatedResult.IsError)
            {
                return Problem(
                    statusCode: 400,
                    title: updatedResult.FirstError.Code,
                    detail: updatedResult.FirstError.Description
                    );
            }
            await work.SaveChangesAsync();

            var updatedCategory = mapper.Map<CategoryDTO>(result.Value);
            return Ok(new ResponseApi(200, "Category updated successfully", updatedCategory));
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var result = await work.Categories.GetByIdAsync(id);
            if (result.IsError)
            {
                return Problem(
                    statusCode: result.FirstError.Type == ErrorType.NotFound?404:400,
                    title: result.FirstError.Code,
                    detail: result.FirstError.Description
                    );
            }
            var deletedResult = await work.Categories.DeleteAsync(id);
            if (deletedResult.IsError)
            {
                return Problem(
                   statusCode: 400,
                   title: deletedResult.FirstError.Code,
                   detail: deletedResult.FirstError.Description
                   );
            }
            await work.SaveChangesAsync();
            return Ok(new ResponseApi(200, "Category deleted successfuly"));
        }
    }
}
