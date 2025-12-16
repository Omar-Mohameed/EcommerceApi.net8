using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ecom.Core.Entities;
using Ecom.Core.Services;

namespace Ecom.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            this.paymentService = paymentService;
        }
        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> CreateOrUpdatePaymentIntent(string basketid, [FromQuery] int? delivertMethodId)
        {
            //var basket = await paymentService.CreateOrUpdatePaymentAsync(basketid, delivertMethodId);
            var UrlPayment = await paymentService.CreateCheckoutSessionAsync(basketid, delivertMethodId);
            if (UrlPayment == null) return BadRequest(new { error = "Problem with your basket" });
            return Ok(UrlPayment);
        }
    }
}
