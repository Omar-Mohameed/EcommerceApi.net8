using AutoMapper;
using ECommerce.Api.Helper;
using ECommerce.Core.Entities;
using ECommerce.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers
{
    public class BasketsController : BaseController
    {
        public BasketsController(IUnitOfWork work, IMapper mapper) : base(work, mapper)
        {
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var result = await work.Baskets.GetBasketAsync(id);
            if (result != null)
                return Ok(result);
            return Ok(new CustomerBasket(id));
        }
        [HttpPut]
        public async Task<IActionResult> Update([FromBody]CustomerBasket basket)
        {
            var result = await work.Baskets.UpdateBasketAsync(basket);
            return Ok(result);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var deleted = await work.Baskets.DeleteBasketAsync(id);
            return deleted ? Ok(new ResponseApi(200, "Deleted Successfully"))
                : BadRequest(new ResponseApi(404, "Basket Not Found"));
        }
    }
}
