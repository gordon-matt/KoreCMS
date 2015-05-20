using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Kore.Data;
using Kore.Exceptions;
using Kore.Infrastructure;
using Kore.Plugins.Ecommerce.Simple.Data.Domain;
using Kore.Plugins.Ecommerce.Simple.Models;
using Kore.Plugins.Ecommerce.Simple.Services;
using Kore.Web.Mvc;

namespace Kore.Plugins.Ecommerce.Simple.Controllers
{
    [RouteArea(Constants.RouteArea)]
    [RoutePrefix("paypal")]
    public class PayPalController : KoreController
    {
        private readonly Lazy<IOrderService> orderService;
        private readonly Lazy<IProductService> productService;
        private readonly PayPalSettings settings;

        public PayPalController(
            Lazy<IOrderService> orderService,
            Lazy<IProductService> productService,
            PayPalSettings settings)
        {
            this.orderService = orderService;
            this.productService = productService;
            this.settings = settings;
        }

        [Route("return/{orderId}")]
        public ActionResult Return(int orderId)
        {
            var order = orderService.Value.FindOne(orderId);

            if (order == null)
            {
                Logger.ErrorFormat("Could not find order number: {0}", orderId);
                return RedirectToAction("Index", "Store", new { area = string.Empty });
            }

            order.Status = OrderStatus.Processing;
            order.PaymentStatus = PaymentStatus.Paid;

            return View(order);
        }

        [Route("cancel-return/{orderId}")]
        public ActionResult CancelReturn(int orderId)
        {
            var order = orderService.Value.FindOne(orderId);

            if (order == null)
            {
                Logger.ErrorFormat("Could not find order number: {0}", orderId);
                return RedirectToAction("Index", "Store", new { area = string.Empty });
            }

            order.Status = OrderStatus.Cancelled;

            return View(order);
        }

        [Route("notification/{orderId}")]
        public ActionResult Notification(int orderId)
        {
            var order = orderService.Value.FindOne(orderId);

            if (order == null)
            {
                Logger.ErrorFormat("Could not find order number: {0}", orderId);
                return new EmptyResult();
            }

            byte[] param = Request.BinaryRead(Request.ContentLength);
            string formParams = Encoding.ASCII.GetString(param);

            Logger.InfoFormat("Notification received. Form params are: {0}", formParams);

            Dictionary<string, string> values;
            if (VerifyIPN(formParams, out values))
            {
                #region Values

                decimal total = decimal.Zero;
                decimal.TryParse(values["mc_gross"], NumberStyles.Any, new CultureInfo("en-US"), out total);
                //try
                //{
                //    total = decimal.Parse(values["mc_gross"], new CultureInfo("en-US"));
                //}
                //catch { }

                string invoice = string.Empty;
                string mcCurrency = string.Empty;
                string payerId = string.Empty;
                string payerStatus = string.Empty;
                string paymentFee = string.Empty;
                string paymentStatus = string.Empty;
                string paymentType = string.Empty;
                string pendingReason = string.Empty;
                string receiverId = string.Empty;
                string rpInvoiceId = string.Empty;
                string txnId = string.Empty;
                string txnType = string.Empty;
                values.TryGetValue("invoice", out invoice);
                values.TryGetValue("mc_currency", out mcCurrency);
                values.TryGetValue("payer_id", out payerId);
                values.TryGetValue("payer_status", out payerStatus);
                values.TryGetValue("payment_fee", out paymentFee);
                values.TryGetValue("payment_status", out paymentStatus);
                values.TryGetValue("payment_type", out paymentType);
                values.TryGetValue("pending_reason", out pendingReason);
                values.TryGetValue("receiver_id", out receiverId);
                values.TryGetValue("rp_invoice_id", out rpInvoiceId);
                values.TryGetValue("txn_id", out txnId);
                values.TryGetValue("txn_type", out txnType);

                #endregion Values

                var sb = new StringBuilder();
                sb.AppendLine("Paypal IPN:");
                foreach (var keyValue in values)
                {
                    sb.Append(keyValue.Key);
                    sb.Append(": ");
                    sb.AppendLine(keyValue.Value);
                }

                var newPaymentStatus = GetPaymentStatus(paymentStatus, pendingReason);
                sb.AppendLine("New payment status: " + newPaymentStatus);

                switch (txnType)
                {
                    case "recurring_payment_profile_created":
                        //do nothing here
                        break;

                    case "recurring_payment":

                        #region Recurring payment

                        {
                            Logger.Error("PayPal IPN. Recurring Payments Not Supported.", new KoreException(sb.ToString()));
                        }

                        #endregion Recurring payment

                        break;

                    default:

                        #region Standard payment

                        {
                            if (order != null)
                            {
                                //order note
                                order.Notes.Add(new OrderNote()
                                {
                                    Text = sb.ToString(),
                                    DisplayToCustomer = false,
                                    DateCreatedUtc = DateTime.UtcNow
                                });
                                orderService.Value.Update(order);

                                switch (newPaymentStatus)
                                {
                                    case PaymentStatus.Authorized:
                                        {
                                            if (CanMarkOrderAsAuthorized(order))
                                            {
                                                order.PaymentStatus = PaymentStatus.Authorized;

                                                //add a note
                                                order.Notes.Add(new OrderNote()
                                                {
                                                    Text = "Order has been marked as authorized",
                                                    DisplayToCustomer = false,
                                                    DateCreatedUtc = DateTime.UtcNow
                                                });
                                                orderService.Value.Update(order);
                                            }
                                        }
                                        break;

                                    case PaymentStatus.Paid:
                                        {
                                            if (CanMarkOrderAsPaid(order))
                                            {
                                                order.AuthorizationTransactionId = txnId;
                                                order.PaymentStatus = PaymentStatus.Paid;
                                                order.DatePaidUtc = DateTime.UtcNow;

                                                //add a note
                                                order.Notes.Add(new OrderNote()
                                                {
                                                    Text = "Order has been marked as paid",
                                                    DisplayToCustomer = false,
                                                    DateCreatedUtc = DateTime.UtcNow
                                                });
                                                orderService.Value.Update(order);
                                            }
                                        }
                                        break;

                                    case PaymentStatus.Refunded:
                                        {
                                            Logger.ErrorFormat("Refunds not yet supported by Simple Commerce. Order ID: {0}", orderId);
                                            //if (CanRefundOffline(order))
                                            //{
                                            //    RefundOffline(order);
                                            //}
                                        }
                                        break;

                                    case PaymentStatus.Voided:
                                        {
                                            Logger.ErrorFormat("Voiding payments not yet supported by Simple Commerce. Order ID: {0}", orderId);
                                            //if (CanVoidOffline(order))
                                            //{
                                            //    VoidOffline(order);
                                            //}
                                        }
                                        break;

                                    case PaymentStatus.Pending:
                                    default: break;
                                }
                            }
                            else
                            {
                                Logger.Error("PayPal IPN. Order is not found", new KoreException(sb.ToString()));
                            }
                        }

                        #endregion Standard payment

                        break;
                }
            }
            else
            {
                Logger.Error("PayPal IPN failed.", new KoreException(formParams));
            }

            return new EmptyResult();
        }

        [Route("buy-now")]
        public ActionResult BuyNow()
        {
            var cart = GetCart();

            var order = new Order
            {
                UserId = WorkContext.CurrentUser == null ? null : WorkContext.CurrentUser.Id,
                PaymentStatus = PaymentStatus.Pending,
                Status = OrderStatus.Pending,
                OrderDateUtc = DateTime.UtcNow,
            };

            //TEMP //TODO; Remove this asap
            var addressRepository = EngineContext.Current.Resolve<IRepository<Address>>();
            var address = addressRepository.Table.FirstOrDefault();

            if (address == null)
            {
                address = new Address
                {
                    FamilyName = "Nissan",
                    GivenNames = "Ran",
                    AddressLine1 = "Line 1",
                    AddressLine2 = "Line 2",
                    AddressLine3 = "Line 3",
                    City = "Tel Aviv",
                    Country = "Israel",
                    PostalCode = "11111",
                    Email = "rn@esales.co.il",
                    PhoneNumber = "0987654321",
                };
                addressRepository.Insert(address);
            }

            order.BillingAddressId = address.Id;
            order.ShippingAddressId = address.Id;
            //END TEMP

            foreach (var item in cart)
            {
                order.Lines.Add(new OrderLine
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.Price
                });
            }

            orderService.Value.Insert(order);

            var model = new PayPalModel
            {
                PassProductNamesAndTotals = true,
                Merchant = settings.Merchant,
                UseSandboxMode = settings.UseSandboxMode,
                ActionUrl = settings.UseSandboxMode
                    ? settings.SandboxUrl
                    : settings.ProductionUrl,
                ReturnUrl = Url.AbsoluteAction("Return", "PayPal", new { orderId = order.Id }),
                CancelReturnUrl = Url.AbsoluteAction("CancelReturn", "PayPal", new { orderId = order.Id }),
                NotificationUrl = Url.AbsoluteAction("Notification", "PayPal", new { orderId = order.Id }),
                CurrencyCode = settings.CurrencyCode,
                Items = cart,
                OrderId = order.Id,
                OrderTotal = order.OrderTotal,
                SalesTax = cart.Sum(x => x.Tax),
                ShippingFee = cart.Sum(x => x.ShippingCost),
                BillingAddress = order.BillingAddress
            };
            return View(model);
        }

        private ICollection<ShoppingCartItem> GetCart()
        {
            ICollection<ShoppingCartItem> cart;

            if (Session.Keys.OfType<string>().Contains("ShoppingCart"))
            {
                cart = (ICollection<ShoppingCartItem>)Session["ShoppingCart"];
            }
            else
            {
                cart = new List<ShoppingCartItem>();
            }

            return cart;
        }

        private bool VerifyIPN(string formParams, out Dictionary<string, string> values)
        {
            string payPalUrl = settings.UseSandboxMode
                ? settings.SandboxUrl
                : settings.ProductionUrl;

            var request = (HttpWebRequest)WebRequest.Create(payPalUrl);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            string formContent = string.Format("{0}&cmd=_notify-validate", formParams);
            request.ContentLength = formContent.Length;

            using (var sw = new StreamWriter(request.GetRequestStream(), Encoding.ASCII))
            {
                sw.Write(formContent);
            }

            string response = null;
            using (var reader = new StreamReader(request.GetResponse().GetResponseStream()))
            {
                response = HttpUtility.UrlDecode(reader.ReadToEnd());
            }

            bool verified = response.Trim().Equals("VERIFIED", StringComparison.OrdinalIgnoreCase);

            values = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (string s in formParams.Split('&'))
            {
                string line = s.Trim();
                int equalPos = line.IndexOf('=');
                if (equalPos >= 0)
                {
                    values.Add(line.Substring(0, equalPos), line.Substring(equalPos + 1));
                }
            }

            return verified;
        }

        /// <summary>
        /// Gets a payment status
        /// </summary>
        /// <param name="paymentStatus">PayPal payment status</param>
        /// <param name="pendingReason">PayPal pending reason</param>
        /// <returns>Payment status</returns>
        private static PaymentStatus GetPaymentStatus(string paymentStatus, string pendingReason)
        {
            var result = PaymentStatus.Pending;

            if (paymentStatus == null)
                paymentStatus = string.Empty;

            if (pendingReason == null)
                pendingReason = string.Empty;

            switch (paymentStatus.ToLowerInvariant())
            {
                case "pending":
                    switch (pendingReason.ToLowerInvariant())
                    {
                        case "authorization":
                            result = PaymentStatus.Authorized;
                            break;

                        default:
                            result = PaymentStatus.Pending;
                            break;
                    }
                    break;

                case "processed":
                case "completed":
                case "canceled_reversal":
                    result = PaymentStatus.Paid;
                    break;

                case "denied":
                case "expired":
                case "failed":
                case "voided":
                    result = PaymentStatus.Voided;
                    break;

                case "refunded":
                case "reversed":
                    result = PaymentStatus.Refunded;
                    break;

                default:
                    break;
            }
            return result;
        }

        private bool CanMarkOrderAsAuthorized(Order order)
        {
            if (order.Status == OrderStatus.Cancelled)
            {
                return false;
            }

            if (order.PaymentStatus == PaymentStatus.Pending)
            {
                return true;
            }

            return false;
        }

        private bool CanMarkOrderAsPaid(Order order)
        {
            if (order.Status == OrderStatus.Cancelled)
            {
                return false;
            }

            if (order.PaymentStatus.In(PaymentStatus.Paid, PaymentStatus.Refunded, PaymentStatus.Voided))
            {
                return false;
            }

            return true;
        }
    }
}