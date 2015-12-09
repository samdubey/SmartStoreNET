using Antargyan.PayU.Helpers;
using Antargyan.PayU.Models;
using Antargyan.PayU.Settings;
using SmartStore;
using SmartStore.Core.Domain.Orders;
using SmartStore.Core.Domain.Payments;
using SmartStore.Services.Configuration;
using SmartStore.Services.Orders;
using SmartStore.Services.Payments;
using SmartStore.Web.Framework.Controllers;
using SmartStore.Web.Framework.Security;
using SmartStore.Web.Models.Common;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Antargyan.PayU.Controllers
{
    public class PayUController : PaymentControllerBase
    {
        private readonly ISettingService _settingService;
        private readonly IPaymentService _paymentService;
        private readonly IOrderService _orderService;
        private readonly IOrderProcessingService _orderProcessingService;
        private readonly PayUSettings _payuSettings;
        private readonly PaymentSettings _paymentSettings;

        public PayUController(ISettingService settingService, IPaymentService paymentService, IOrderService orderService, IOrderProcessingService orderProcessingService, PayUSettings PayuPaymentSettings, PaymentSettings paymentSettings)
        {

            this._settingService = settingService;
            this._paymentService = paymentService;
            this._orderService = orderService;
            this._orderProcessingService = orderProcessingService;
            this._payuSettings = PayuPaymentSettings;
            this._paymentSettings = paymentSettings;
        }
        
        [AdminAuthorize]
        [ChildActionOnly]
        public ActionResult Configure()
        {
            return View("Configure", (object)new ConfigurationModel()
            {
                Key = this._payuSettings.Key,
                Salt = this._payuSettings.Salt,
                MerchantParam = this._payuSettings.MerchantParam,
                PayUri = this._payuSettings.PayUri,
                AdditionalFee = this._payuSettings.AdditionalFee,
                IsPayUBiz=true
            });
        }

        [HttpPost]
        [ChildActionOnly]
        [AdminAuthorize]
        public ActionResult Configure(ConfigurationModel model)
        {
            if (!((Controller)this).ModelState.IsValid)
                return this.Configure();
            this._payuSettings.Key = model.Key;
            this._payuSettings.Salt = model.Salt;
            this._payuSettings.MerchantParam = model.MerchantParam;
            this._payuSettings.PayUri = model.PayUri;
            this._payuSettings.AdditionalFee = model.AdditionalFee;
            this._settingService.SaveSetting<PayUSettings>(_payuSettings, 0);
            return View("Configure", (object)model);
        }

        [ChildActionOnly]
        public ActionResult PaymentInfo()
        {
            return View("PaymentInfo", new Antargyan.PayU.Models.PaymentInfoModel());
        }

        [NonAction]
        public override IList<string> ValidatePaymentForm(FormCollection form)
        {
            return (IList<string>)new List<string>();
        }

        [NonAction]
        public override ProcessPaymentRequest GetPaymentInfo(FormCollection form)
        {
            return new ProcessPaymentRequest();
        }

        [ValidateInput(false)]
        public ActionResult Return(FormCollection form)
        {
            PayUHelper payuHelper = new PayUHelper();
            if (string.IsNullOrWhiteSpace(this._payuSettings.Salt))
                throw new SmartException("PayU key is not set");
            string MerchantId = ((object)this._payuSettings.Key).ToString();
            string OrderId = form["txnid"];
            string Amount = form["amount"];
            string productinfo = form["productinfo"];
            string firstname = form["firstname"];
            string email = form["email"];
            string str = form["hash"];
            string status = form["status"];
            if (!(payuHelper.verifychecksum(MerchantId, OrderId, Amount, productinfo, firstname, email, status, this._payuSettings.Salt) == str))
                return Content("Security Error. Illegal access detected");
            if (!(status == "success"))
                return Content("Thank you for shopping with us. However, the transaction has been declined");
            Order orderById = this._orderService.GetOrderById(Convert.ToInt32(OrderId));
            if (this._orderProcessingService.CanMarkOrderAsPaid(orderById))
                this._orderProcessingService.MarkOrderAsPaid(orderById);
            return RedirectToAction("Completed", "Checkout", new { area = "" });
        }
    }
}
