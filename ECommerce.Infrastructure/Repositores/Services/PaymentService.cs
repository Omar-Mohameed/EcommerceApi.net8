using ECommerce.Core.Entities;
using ECommerce.Core.Interfaces;
using ECommerce.Core.Services;
using ECommerce.Core.Shared;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Repositores.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork work;
        private readonly AppDbContext _context;
        private readonly StripeSettings _settings;
        private readonly string ApiKey;
        private readonly string PublishKey;

        public PaymentService(IUnitOfWork work, IOptions<StripeSettings> stripeSettings, AppDbContext context)
        {
            this.work = work;
            _settings = stripeSettings.Value;
            ApiKey = _settings.SecretKey;
            PublishKey = _settings.PublishableKey;
            _context = context;
        }

        // V1 - Stripe Payment Intent ( returns Client Secret Payment from stripe to front end and store PaymentIntentId in basket)
        public async Task<CustomerBasket> CreateOrUpdatePaymentAsync(string basketid, int? delivertMethodId)
        {
            // Get Basket from redis
            var basket = await work.Baskets.GetBasketAsync(basketid);
            if (basket == null) return null;
            // 2) Get Stripe Keys
            StripeConfiguration.ApiKey = ApiKey;
            // calc shipping price if found
            decimal shippingPrice = 0m;
            if (delivertMethodId.HasValue)
            {
                var delivery = await _context.DeliveryMethods.AsNoTracking()
                    .FirstOrDefaultAsync(m => m.Id == delivertMethodId.Value);
                shippingPrice = delivery.Price;
            }
            foreach(var item in basket.basketItems)
            {
                var product = await work.Products.GetByIdAsync(item.Id);
                item.Price = product.Value.NewPrice;
            }
            //// calc total price
            //var ItemsPrice = basket.basketItems.Sum(item => item.Price * item.Qunatity * 100); // *100 =z> Cents
            //var TotalPrice = shippingPrice + ItemsPrice;

            PaymentIntentService paymentIntentService = new PaymentIntentService(); // انشاء العمليه الماليه service from stripe to deal with payment action
            PaymentIntent _intent; //اوبجيكن هنحط فيه عملية الدفع ال استرايب هيرجعها 

            // 4) Create new payment intent لو محصلش عمليه دفع عالسله دي خالص
            if (string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = (long)basket.basketItems.Sum(item => item.Qunatity * (item.Price * 100)) + (long)(shippingPrice * 100),

                    Currency = "USD",

                    PaymentMethodTypes = new List<string> { "card"}
                };
                // create paymentintent in stripe and store the result in intent obj
                _intent = await paymentIntentService.CreateAsync(options); 
                basket.PaymentIntentId = _intent.Id;
                basket.ClientSecret = _intent.ClientSecret;
            }
            else
            {
                var options = new PaymentIntentUpdateOptions
                {
                    Amount = (long)basket.basketItems.Sum(item => item.Qunatity * (item.Price * 100)) + (long)(shippingPrice * 100),
                };
                await paymentIntentService.UpdateAsync(basket.PaymentIntentId,options);
            }
            await work.Baskets.UpdateBasketAsync(basket);
            return basket;
        }



        // V2 - Stripe Checkout Session(return Url Payment from stripe to front end)
        public async Task<string> CreateCheckoutSessionAsync(string basketId, int? deliveryMethodId)
        {
            // 1) Get Basket
            var basket = await work.Baskets.GetBasketAsync(basketId);
            if (basket == null) return null;

            // 2) Stripe Api Key
            StripeConfiguration.ApiKey = ApiKey;

            // 3) Shipping Price
            decimal shippingPrice = 0m;
            if (deliveryMethodId.HasValue)
            {
                var delivery = await _context.DeliveryMethods
                    .AsNoTracking()
                    .FirstOrDefaultAsync(m => m.Id == deliveryMethodId.Value);

                shippingPrice = delivery.Price;
            }

            // 4) Update product prices from DB
            foreach (var item in basket.basketItems)
            {
                var product = await work.Products.GetByIdAsync(item.Id);
                item.Price = product.Value.NewPrice;
            }

            // ------------------------------
            // ⭐ 5) Create checkout session
            // ------------------------------

            var domain = "https://localhost:7025"; // FRONT END URL

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },

                Mode = "payment",

                SuccessUrl = domain + "/success?session_id={CHECKOUT_SESSION_ID}",
                CancelUrl = domain + "/cancel",

                LineItems = new List<SessionLineItemOptions>()
            };

            // items
            foreach (var item in basket.basketItems)
            {
                options.LineItems.Add(new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Price * 100), // cents
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Name
                        }
                    },
                    Quantity = item.Qunatity
                });
            }

            // shipping as extra line item
            if (shippingPrice > 0)
            {
                options.LineItems.Add(new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(shippingPrice * 100),
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = "Shipping Cost"
                        }
                    },
                    Quantity = 1
                });
            }

            // create session
            var service = new SessionService();
            var session = await service.CreateAsync(options);

            // optionally store in basket
            basket.PaymentIntentId = session.Id; // Not required but useful
            await work.Baskets.UpdateBasketAsync(basket);

            return session.Url; // return link to the user
        }

    }
}

/*
    Flow  

Add to Cart (Redis)
        ↓
Checkout
        ↓
Create PaymentIntent (stripe)
        ↓
User Pays
        ↓
If Success → Create Order in DB
        ↓
Delete Basket
        ↓
Return order details



1-العميل يختار المنتجات

إضافتها للسلة-2

3-Checkout

4-Backend ينشئ/يحدث PaymentIntent

5-Stripe يعرض الدفع

6-Stripe يخصم الفلوس

7-Backend ينشئ Order

يحذف Basket-8

يرجع9 Order للعميل
 */