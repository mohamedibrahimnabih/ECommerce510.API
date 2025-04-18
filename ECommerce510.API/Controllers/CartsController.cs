using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce510.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICartRepository _cartRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderItemRepository _orderItemRepository;

        public CartsController(UserManager<ApplicationUser> userManager, ICartRepository cartRepository, IOrderRepository orderRepository, IOrderItemRepository orderItemRepository)
        {
            _userManager = userManager;
            _cartRepository = cartRepository;
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> AddToCart([FromRoute] int id, [FromQuery] int count, CancellationToken cancellationToken)
        {
            var appUserId = _userManager.GetUserId(User);

            if (appUserId is not null)
            {
                Cart cart = new Cart()
                {
                    ApplicationUserId = appUserId,
                    ProductId = id,
                    Count = count
                };

                var cartInDb = _cartRepository.GetOne(e => e.ApplicationUserId == appUserId && e.ProductId == id);

                if (cartInDb != null)
                    cartInDb.Count += count;
                else
                    await _cartRepository.CreateAsync(cart, cancellationToken);

                return NoContent();
            }


            return NotFound();
        }

        [HttpGet("")]
        public IActionResult GetAll()
        {
            var appUserId = _userManager.GetUserId(User);

            var cart = _cartRepository.Get(e => e.ApplicationUserId == appUserId, [e => e.Product, e => e.ApplicationUser]);

            var cartReturned = cart.Adapt<IEnumerable<CartResponse>>().ToList();

            CartWithTotalResponse cartWithTotalResponse = new()
            {
                Carts = cartReturned.ToList(),
                TotalPrice = cart.Sum(e => e.Product.Price * e.Count)
            };

            return Ok(cartWithTotalResponse);
        }

        //public IActionResult Increment(int productId)
        //{
        //    var appUserId = _userManager.GetUserId(User);

        //    var cart = _cartRepository.GetOne(e => e.ApplicationUserId == appUserId && e.ProductId == productId);

        //    cart.Count++;
        //    _cartRepository.Commit();

        //    return RedirectToAction("Index");
        //}

        //public IActionResult Decrement(int productId)
        //{
        //    var appUserId = _userManager.GetUserId(User);

        //    var cart = _cartRepository.GetOne(e => e.ApplicationUserId == appUserId && e.ProductId == productId);

        //    if (cart.Count > 1)
        //    {
        //        cart.Count--;
        //        _cartRepository.Commit();
        //    }

        //    return RedirectToAction("Index");
        //}

        //public IActionResult Pay()
        //{
        //    var appUserId = _userManager.GetUserId(User);

        //    var carts = _cartRepository.Get(e => e.ApplicationUserId == appUserId, [e => e.Product]);

        //    Order order = new();
        //    order.ApplicationUserId = appUserId;
        //    order.OrderDate = DateTime.Now;
        //    order.OrderTotal = carts.Sum(e => e.Product.Price * e.Count);
        //    order.PaymentStatus = false;
        //    order.Status = OrderStatus.Pending;

        //    _orderRepository.Create(order);
        //    _orderRepository.Commit();

        //    var options = new SessionCreateOptions
        //    {
        //        PaymentMethodTypes = new List<string> { "card" },
        //        LineItems = new List<SessionLineItemOptions>(),
        //        Mode = "payment",
        //        SuccessUrl = $"{Request.Scheme}://{Request.Host}/Customer/Checkout/Success?orderId={order.OrderId}",
        //        CancelUrl = $"{Request.Scheme}://{Request.Host}/Customer/Checkout/Cancel",
        //    };

        //    foreach (var item in carts)
        //    {
        //        options.LineItems.Add(new SessionLineItemOptions
        //        {
        //            PriceData = new SessionLineItemPriceDataOptions
        //            {
        //                Currency = "egp",
        //                ProductData = new SessionLineItemPriceDataProductDataOptions
        //                {
        //                    Name = item.Product.Name,
        //                    Description = item.Product.Description,
        //                },
        //                UnitAmount = (long)item.Product.Price * 100,
        //            },
        //            Quantity = item.Count,
        //        });
        //    }

        //    var service = new SessionService();
        //    var session = service.Create(options);
        //    order.SessionId = session.Id;

        //    _orderRepository.Commit();

        //    return Redirect(session.Url);
        //}
    }
}
