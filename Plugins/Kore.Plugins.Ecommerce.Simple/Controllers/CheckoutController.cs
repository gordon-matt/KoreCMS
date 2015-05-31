using System;
using System.Linq;
using System.Web.Mvc;
using Kore.Plugins.Ecommerce.Simple.Data.Domain;
using Kore.Plugins.Ecommerce.Simple.Models;
using Kore.Plugins.Ecommerce.Simple.Services;
using Kore.Web.Mvc;
using Kore.Web.Mvc.Optimization;

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

        [Compress]
        [Route("")]
        public ActionResult Index()
        {
            WorkContext.Breadcrumbs.Add(T(LocalizableStrings.Store), Url.Action("Index", "Store", new { area = string.Empty }));
            WorkContext.Breadcrumbs.Add(T(LocalizableStrings.Checkout));

            ViewBag.Title = T(LocalizableStrings.Store);
            ViewBag.SubTitle = T(LocalizableStrings.Checkout);

            ViewBag.CurrencyCode = settings.Currency;

            var model = new CheckoutModel
            {
                Cart = cartService.Value.GetCart(this.HttpContext)
            };

            return View(model);
        }

        [HttpPost]
        [Route("confirm")]
        public ActionResult Confirm(CheckoutModel model)
        {
            var cart = cartService.Value.GetCart(this.HttpContext);

            Address billingAddress;
            Address shippingAddress;

            #region Get Billing Address

            billingAddress = addressService.Value.FindOne(x =>
                x.FamilyName == model.BillingAddress.FamilyName &&
                x.GivenNames == model.BillingAddress.GivenNames &&
                x.Email == model.BillingAddress.Email &&
                x.AddressLine1 == model.BillingAddress.AddressLine1 &&
                x.AddressLine2 == model.BillingAddress.AddressLine2 &&
                x.AddressLine3 == model.BillingAddress.AddressLine3 &&
                x.City == model.BillingAddress.City &&
                x.PostalCode == model.BillingAddress.PostalCode &&
                x.CountryId == model.BillingAddress.CountryId);

            if (billingAddress == null)
            {
                billingAddress = new Address
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
                    CountryId = model.BillingAddress.CountryId
                };

                if (WorkContext.CurrentUser != null)
                {
                    billingAddress.UserId = WorkContext.CurrentUser.Id;
                }

                addressService.Value.Insert(billingAddress);
            }
            else
            {
                // User may have been anonymous before, but logged in now.. so we can add UserId.
                if (string.IsNullOrEmpty(billingAddress.UserId) && WorkContext.CurrentUser != null)
                {
                    billingAddress.UserId = WorkContext.CurrentUser.Id;
                    addressService.Value.Update(billingAddress);
                }
            }

            #endregion Get Billing Address

            #region Get Shipping Address

            if (model.ShippingAddressIsSameAsBillingAddress)
            {
                shippingAddress = billingAddress;
            }
            else
            {
                shippingAddress = addressService.Value.FindOne(x =>
                    x.FamilyName == model.ShippingAddress.FamilyName &&
                    x.GivenNames == model.ShippingAddress.GivenNames &&
                    x.Email == model.ShippingAddress.Email &&
                    x.AddressLine1 == model.ShippingAddress.AddressLine1 &&
                    x.AddressLine2 == model.ShippingAddress.AddressLine2 &&
                    x.AddressLine3 == model.ShippingAddress.AddressLine3 &&
                    x.City == model.ShippingAddress.City &&
                    x.PostalCode == model.ShippingAddress.PostalCode &&
                    x.CountryId == model.ShippingAddress.CountryId);

                if (shippingAddress == null)
                {
                    shippingAddress = new Address
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
                        CountryId = model.ShippingAddress.CountryId
                    };

                    if (WorkContext.CurrentUser != null)
                    {
                        shippingAddress.UserId = WorkContext.CurrentUser.Id;
                    }

                    addressService.Value.Insert(shippingAddress);
                }
            }

            #endregion Get Shipping Address

            var order = new Order
            {
                UserId = WorkContext.CurrentUser == null ? null : WorkContext.CurrentUser.Id,
                PaymentStatus = PaymentStatus.Pending,
                Status = OrderStatus.Pending,
                OrderDateUtc = DateTime.UtcNow,
                IPAddress = ClientIPAddress,
                OrderTotal = cart.Items.Sum(x => (x.Price + x.Tax + x.ShippingCost) * x.Quantity),
                BillingAddressId = billingAddress.Id,
                ShippingAddressId = shippingAddress.Id
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

            return RedirectToAction("BuyNow", "PayPal", new { area = string.Empty });
        }

        [Route("completed/{orderId}")]
        public ActionResult Completed(int orderId)
        {
            WorkContext.Breadcrumbs.Add(T(LocalizableStrings.Store), Url.Action("Index", "Store", new { area = string.Empty }));
            WorkContext.Breadcrumbs.Add(T(LocalizableStrings.Checkout));
            WorkContext.Breadcrumbs.Add(T(LocalizableStrings.Completed));

            var order = orderService.Value.FindOne(orderId);
            return View(order);
        }
    }
}