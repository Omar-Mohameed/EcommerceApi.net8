using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ecom.Core.Interfaces;

namespace Ecom.Api.Controllers
{
    public class BugController : BaseController
    {
        public BugController(IUnitOfWork work, IMapper mapper) : base(work, mapper)
        {
        }
        [HttpGet("not-found")]
        public async Task<ActionResult> GetNotFound()
        {
            var category = await work.Categories.GetByIdAsync(100);
            if (category.IsError) return NotFound();
            return Ok(category);
        }

        [HttpGet("server-error")]
        public async Task<ActionResult> GetServerError()
        {
            var category = await work.Categories.GetByIdAsync(100);
            category.Value.Name = "";// 
            return Ok(category);
        }

        [HttpGet("bad-request/{Id}")]
        public async Task<ActionResult> GetBadRequest(int id)
        {
            return Ok();
        }

        [HttpGet("bad-request/")]
        public async Task<ActionResult> GetBadRequest()
        {
            return BadRequest();
        }


    }
}
