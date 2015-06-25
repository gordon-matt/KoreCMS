using System.Data.Entity;
using System;
using System.Linq;
using System.Web.Mvc;
using Kore.Collections;
using Kore.Plugins.Ecommerce.Simple.Data.Domain;
using Kore.Plugins.Ecommerce.Simple.Models;
using Kore.Plugins.Ecommerce.Simple.Services;
using Kore.Web.Common.Areas.Admin.Regions.Domain;
using Kore.Web.Common.Areas.Admin.Regions.Services;
using Kore.Web.Mvc;
using Kore.Web.Mvc.Optimization;
using Kore.Web;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Kore.Plugins.Ecommerce.Simple.Controllers
{
    [RouteArea("")]
    [RoutePrefix("store/checkout")]
    public class CheckoutController : KoreController
    {
        private readonly Lazy<IAddressService> addressService;
        private readonly Lazy<ICartService> cartService;
        private readonly Lazy<IOrderService> orderService;
        private readonly Lazy<IRegionService> regionService;
        private readonly Lazy<IRegionSettingsService> regionSettingsService;
        private readonly StoreSettings settings;

        public CheckoutController(
            Lazy<IAddressService> addressService,
            Lazy<ICartService> cartService,
            Lazy<IOrderService> orderService,
            Lazy<IRegionService> regionService,
            Lazy<IRegionSettingsService> regionSettingsService,
            StoreSettings settings)
        {
            this.addressService = addressService;
            this.cartService = cartService;
            this.orderService = orderService;
            this.regionService = regionService;
            this.regionSettingsService = regionSettingsService;
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

            var cart = cartService.Value.GetCart(this.HttpContext);

            var model = new CheckoutModel
            {
                Cart = cart,
                SubTotal = cart.Items.Sum(x => (x.Price * x.Quantity)),
                TaxTotal = cart.Items.Sum(x => (x.Tax * x.Quantity)),
                ShippingTotal = settings.ShippingFlatRate + (cart.Items.Sum(x => (x.ShippingCost * x.Quantity))),
                GrandTotal = settings.ShippingFlatRate + (cart.Items.Sum(x => (x.Price + x.Tax + x.ShippingCost) * x.Quantity))
            };

            return View(model);
        }

        [HttpPost]
        [Route("confirm")]
        public ActionResult Confirm(CheckoutModel model)
        {
            var cart = cartService.Value.GetCart(this.HttpContext);

            if (cart.Items.Count == 0)
            {
                RedirectToAction("Index");
            }

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
                x.CityId == model.BillingAddress.CityId &&
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
                    CityId = model.BillingAddress.CityId,
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
                    x.CityId == model.ShippingAddress.CityId &&
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
                        CityId = model.ShippingAddress.CityId,
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

            #region Calculate Shipping Cost

            float shippingTotal = settings.ShippingFlatRate + cart.Items.Sum(x => (x.ShippingCost * x.Quantity));

            //float shippingTotal = 0f;

            //// If there's only 1 item...
            //if (cart.Items.Count == 1)
            //{
            //    var item = cart.Items.First();

            //    // If that item has a shipping cost specified...
            //    if (item.ShippingCost != 0f)
            //    {
            //        // ...then use it
            //        shippingTotal = (item.ShippingCost * item.Quantity);
            //    }
            //    else
            //    {
            //        // ...else use flat rate
            //        shippingTotal = settings.ShippingFlatRate;
            //    }
            //}
            //else
            //{
            //    // Else there's more than 1 item in cart, so...
            //    // ... first check if there are any items with no shipping cost specified..
            //    if (cart.Items.Any(x => x.ShippingCost == 0f))
            //    {
            //        // Yes, so we add the flat rate first...
            //        shippingTotal = settings.ShippingFlatRate;
            //    }
            //    // and finally we add the extra shipping rates from each product that specified one
            //    shippingTotal += cart.Items.Sum(x => (x.ShippingCost * x.Quantity));
            //}

            #endregion Calculate Shipping Cost

            float taxtTotal = cart.Items.Sum(x => x.Tax * x.Quantity);
            float orderTotal = cart.Items.Sum(x => x.Price * x.Quantity) + shippingTotal + taxtTotal;

            var order = new Order
            {
                UserId = WorkContext.CurrentUser == null ? null : WorkContext.CurrentUser.Id,
                PaymentStatus = PaymentStatus.Pending,
                Status = OrderStatus.Pending,
                OrderDateUtc = DateTime.UtcNow,
                IPAddress = ClientIPAddress,
                ShippingTotal = shippingTotal,
                TaxTotal = taxtTotal,
                OrderTotal = orderTotal,
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

            var order = orderService.Value.Repository.Table
                .Include(x => x.BillingAddress)
                .Include(x => x.BillingAddress.Country)
                .Include(x => x.BillingAddress.City)
                .Include(x => x.ShippingAddress)
                .Include(x => x.ShippingAddress.Country)
                .Include(x => x.ShippingAddress.City)
                .Include(x => x.Lines)
                .Include(x => x.Lines.Select(y => y.Product))
                .FirstOrDefault(x => x.Id == orderId);

            ViewBag.CurrencyCode = settings.Currency;
            return View(order);
        }

        [Route("get-cities/{countryId}")]
        public JsonResult GetCities(int countryId)
        {
            var cities = regionService.Value
                .GetSubRegions(countryId, RegionType.City)
                .ToDictionary(k => k.Id, v => v);

            string settingsId = StoreRegionSettings.SettingsName.ToSlugUrl();

            var settings = regionSettingsService.Value.Find(x =>
                x.SettingsId == settingsId &&
                cities.Keys.Contains(x.RegionId));

            // If no settings have been made for this plugin,
            if (!settings.Any())
            {
                // ... then we assume ALL cities are OK.
                var data = cities.Values.OrderBy(x => x.Name).Select(x => new
                {
                    Id = x.Id,
                    Name = x.Name
                });

                return Json(new { Data = data }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                // Else only get the cities specified
                var records = new HashSet<Region>();
                foreach (var setting in settings)
                {
                    dynamic fields = JObject.Parse(setting.Fields);
                    bool isEnabled = fields.IsEnabled;

                    if (isEnabled)
                    {
                        records.Add(cities[setting.RegionId]);
                    }
                }

                var data = records.Select(x => new
                {
                    Id = x.Id,
                    Name = x.Name
                });

                return Json(new { Data = data }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}