using Antargyan.PayU.Controllers;
using Antargyan.PayU.Helpers;
using Antargyan.PayU.Settings;
using SmartStore.Core;
using SmartStore.Core.Domain.Directory;
using SmartStore.Core.Domain.Shipping;
using SmartStore.Core.Plugins;
using SmartStore.Services.Directory;
using SmartStore.Services.Payments;
using SmartStore.Web.Framework;
using System;
using System.Globalization;

namespace Antargyan.PayU.Providers
{
    [DisplayOrder(1)]
    [SystemName("Antargyan.PayU")]
    [FriendlyName("Antargyan PayU")]
    public class PayUProvider : PayUProviderBase<PayUSettings>, IConfigurable
    {
        private readonly ICurrencyService _currencyService;
        private readonly CurrencySettings _currencySettings;
        private readonly PayUSettings _payuSettings;
        private readonly IWebHelper _webHelper;

        public PayUProvider(ICurrencyService currencyService, CurrencySettings currencySettings, PayUSettings payuSettings, IWebHelper webHelper)
        {
            this._currencyService = currencyService;
            this._currencySettings = currencySettings;
            this._payuSettings = payuSettings;
            this._webHelper = webHelper;
        }

        protected override string GetActionPrefix()
        {
            return "";
        }
        public override Type GetControllerType()
        {
            return typeof(PayUController);
        }
        public override void GetConfigurationRoute(out string actionName, out string controllerName, out System.Web.Routing.RouteValueDictionary routeValues)
        {
            base.GetConfigurationRoute(out actionName, out controllerName, out routeValues);
        }

        public override void PostProcessPayment(PostProcessPaymentRequest postProcessPaymentRequest)
        {
            var myUtility = new PayUHelper();

            var remotePostHelper = new RemotePost();
            remotePostHelper.FormName = "PayuForm";
            remotePostHelper.Url = _payuSettings.PayUri;
            remotePostHelper.Add("key", _payuSettings.Key.ToString());
            remotePostHelper.Add("amount", postProcessPaymentRequest.Order.OrderTotal.ToString(new CultureInfo("en-US", false).NumberFormat));
            remotePostHelper.Add("productinfo", "productinfo");
            remotePostHelper.Add("Currency", _currencyService.GetCurrencyById(1).CurrencyCode);
            remotePostHelper.Add("Order_Id", postProcessPaymentRequest.Order.Id.ToString());
            remotePostHelper.Add("txnid", postProcessPaymentRequest.Order.Id.ToString());
            if (!_payuSettings.IsPayUBiz)
                remotePostHelper.Add("service_provider", "payu_paisa");
            remotePostHelper.Add("surl", _webHelper.GetStoreLocation(false) + "Plugins/Antargyan.PayU/Return");
            remotePostHelper.Add("furl", _webHelper.GetStoreLocation(false) + "Plugins/Antargyan.PayU/Return");
            remotePostHelper.Add("hash", myUtility.getchecksum(_payuSettings.Key.ToString(),
                postProcessPaymentRequest.Order.Id.ToString(), postProcessPaymentRequest.Order.OrderTotal.ToString(new CultureInfo("en-US", false).NumberFormat),
                "productinfo", postProcessPaymentRequest.Order.BillingAddress.FirstName.ToString(),
                postProcessPaymentRequest.Order.BillingAddress.Email.ToString(), _payuSettings.Salt));


            //Billing details
            remotePostHelper.Add("firstname", postProcessPaymentRequest.Order.BillingAddress.FirstName.ToString());
            remotePostHelper.Add("billing_cust_address", postProcessPaymentRequest.Order.BillingAddress.Address1);
            remotePostHelper.Add("phone", postProcessPaymentRequest.Order.BillingAddress.PhoneNumber);
            remotePostHelper.Add("email", postProcessPaymentRequest.Order.BillingAddress.Email.ToString());
            remotePostHelper.Add("billing_cust_city", postProcessPaymentRequest.Order.BillingAddress.City);
            var billingStateProvince = postProcessPaymentRequest.Order.BillingAddress.StateProvince;
            if (billingStateProvince != null)
                remotePostHelper.Add("billing_cust_state", billingStateProvince.Abbreviation);
            else
                remotePostHelper.Add("billing_cust_state", "");
            remotePostHelper.Add("billing_zip_code", postProcessPaymentRequest.Order.BillingAddress.ZipPostalCode);
            var billingCountry = postProcessPaymentRequest.Order.BillingAddress.Country;
            if (billingCountry != null)
                remotePostHelper.Add("billing_cust_country", billingCountry.ThreeLetterIsoCode);
            else
                remotePostHelper.Add("billing_cust_country", "");

            //Delivery details

            if (postProcessPaymentRequest.Order.ShippingStatus != ShippingStatus.ShippingNotRequired)
            {
                remotePostHelper.Add("delivery_cust_name", postProcessPaymentRequest.Order.ShippingAddress.FirstName);
                remotePostHelper.Add("delivery_cust_address", postProcessPaymentRequest.Order.ShippingAddress.Address1);
                remotePostHelper.Add("delivery_cust_notes", string.Empty);
                remotePostHelper.Add("delivery_cust_tel", postProcessPaymentRequest.Order.ShippingAddress.PhoneNumber);
                remotePostHelper.Add("delivery_cust_city", postProcessPaymentRequest.Order.ShippingAddress.City);
                var deliveryStateProvince = postProcessPaymentRequest.Order.ShippingAddress.StateProvince;
                if (deliveryStateProvince != null)
                    remotePostHelper.Add("delivery_cust_state", deliveryStateProvince.Abbreviation);
                else
                    remotePostHelper.Add("delivery_cust_state", "");
                remotePostHelper.Add("delivery_zip_code", postProcessPaymentRequest.Order.ShippingAddress.ZipPostalCode);
                var deliveryCountry = postProcessPaymentRequest.Order.ShippingAddress.Country;
                if (deliveryCountry != null)
                    remotePostHelper.Add("delivery_cust_country", deliveryCountry.ThreeLetterIsoCode);
                else
                    remotePostHelper.Add("delivery_cust_country", "");
            }
            //remotePostHelper.Add("Merchant_Param", _PayuPaymentSettings.MerchantParam);
            remotePostHelper.Post();
        }

    }
}
