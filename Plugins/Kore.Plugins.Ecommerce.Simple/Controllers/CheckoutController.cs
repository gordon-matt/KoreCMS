using System;
using System.Linq;
using System.Web.Mvc;
using Kore.Plugins.Ecommerce.Simple.Data.Domain;
using Kore.Plugins.Ecommerce.Simple.Models;
using Kore.Plugins.Ecommerce.Simple.Services;
using Kore.Web.Mvc;

namespace Kore.Plugins.Ecommerce.Simple.Controllers
{
    [RouteArea("")]
    [RoutePrefix("store/checkout")]
    public class CheckoutController : KoreController
    {
        private readonly Lazy<IAddressService> addressService;
        private readonly Lazy<ICartService> cartService;
        private readonly Lazy<IOrderService> orderService;
        private readonly StoreSettings settings;

        public CheckoutController(
            Lazy<IAddressService> addressService,
            Lazy<ICartService> cartService,
            Lazy<IOrderService> orderService,
            StoreSettings settings)
        {
            this.addressService = addressService;
            this.cartService = cartService;
            this.orderService = orderService;
            this.settings = settings;
        }

        [Route("")]
        public ActionResult Index()
        {
            WorkContext.Breadcrumbs.Add(T(LocalizableStrings.Store));
            WorkContext.Breadcrumbs.Add(T(LocalizableStrings.Checkout));

            ViewBag.Title = T(LocalizableStrings.Store);
            ViewBag.SubTitle = T(LocalizableStrings.Checkout);

            ViewBag.CurrencyCode = settings.Currency;
            return View();
        }

        [HttpPost]
        [Route("create-order")]
        public JsonResult CreateOrder(CreateOrderModel model)
        {
            try
            {
                var cart = cartService.Value.GetCart(this.HttpContext);

                var order = new Order
                {
                    UserId = WorkContext.CurrentUser == null ? null : WorkContext.CurrentUser.Id,
                    PaymentStatus = PaymentStatus.Pending,
                    Status = OrderStatus.Pending,
                    OrderDateUtc = DateTime.UtcNow,
                    IPAddress = ClientIPAddress,
                    OrderTotal = cart.Items.Sum(x => (x.Price + x.Tax + x.ShippingCost) * x.Quantity),
                    BillingAddress = new Address
                    {
                        FamilyName = model.BillingAddress.FamilyName,
                        GivenNames = model.BillingAddress.GivenNames,
                        UserId = model.BillingAddress.UserId,
                        Email = model.BillingAddress.Email,
                        PhoneNumber = model.BillingAddress.PhoneNumber,
                        AddressLine1 = model.BillingAddress.AddressLine1,
                        AddressLine2 = model.BillingAddress.AddressLine2,
                        AddressLine3 = model.BillingAddress.AddressLine3,
                        City = model.BillingAddress.City,
                        PostalCode = model.BillingAddress.PostalCode,
                        Country = model.BillingAddress.Country
                    },
                    ShippingAddress = new Address
                    {
                        FamilyName = model.ShippingAddress.FamilyName,
                        GivenNames = model.ShippingAddress.GivenNames,
                        UserId = model.ShippingAddress.UserId,
                        Email = model.ShippingAddress.Email,
                        PhoneNumber = model.ShippingAddress.PhoneNumber,
                        AddressLine1 = model.ShippingAddress.AddressLine1,
                        AddressLine2 = model.ShippingAddress.AddressLine2,
                        AddressLine3 = model.ShippingAddress.AddressLine3,
                        City = model.ShippingAddress.City,
                        PostalCode = model.ShippingAddress.PostalCode,
                        Country = model.ShippingAddress.Country
                    }
                };

                foreach (var item in cart.Items)
                {
                    order.Lines.Add(new OrderLine
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        UnitPrice = item.Price
                    });
                }
                orderService.Value.Insert(order);

                cart.OrderId = order.Id;
                cartService.Value.SetCart(this.HttpContext, cart);

                return Json(new { Success = true });
            }
            catch (Exception x)
            {
                Logger.Error(x.Message, x);
                return Json(new { Success = false, Message = x.Message });
            }
        }

        [Route("completed/{orderId}")]
        public ActionResult Completed(int orderId)
        {
            return View(orderId);
        }
    }
}