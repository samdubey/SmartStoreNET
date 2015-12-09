using Antargyan.PayU.Controllers;
using Antargyan.PayU.Settings;
using SmartStore;
using SmartStore.Core;
using SmartStore.Core.Configuration;
using SmartStore.Core.Domain.Orders;
using SmartStore.Core.Domain.Payments;
using SmartStore.Services;
using SmartStore.Services.Orders;
using SmartStore.Services.Payments;
using System;
using System.Collections.Generic;
using System.Web.Routing;

namespace Antargyan.PayU.Providers
{
    public abstract class PayUProviderBase<TSetting> : PaymentMethodBase where TSetting : PaymentSettingsBase, ISettings, new()
    {
        public ICommonServices CommonServices { get; set; }

        public IOrderTotalCalculationService OrderTotalCalculationService { get; set; }

        public override PaymentMethodType PaymentMethodType
        {
            get
            {
                return (PaymentMethodType)15;
            }
        }

        public override bool SupportVoid
        {
            get
            {
                return false;
            }
        }

        public override bool SupportRefund
        {
            get
            {
                return false;
            }
        }

        public override bool SupportCapture
        {
            get
            {
                return false;
            }
        }

        public override bool SupportPartiallyRefund
        {
            get
            {
                return false;
            }
        }

        public override bool RequiresInteraction
        {
            get
            {
                return true;
            }
        }

        public override RecurringPaymentType RecurringPaymentType
        {
            get
            {
                return (RecurringPaymentType)10;
            }
        }

        protected PayUProviderBase()
            : base()
        {
        }

        public override Type GetControllerType()
        {
            return typeof(PayUController);
        }

        protected abstract string GetActionPrefix();

        public override Decimal GetAdditionalHandlingFee(IList<OrganizedShoppingCartItem> cart)
        {
            Decimal num = new Decimal(0);
            try
            {
                this.CommonServices.Settings.LoadSetting<TSetting>(CommonServices.StoreContext.CurrentStore.Id);
                num = PaymentExtentions.CalculateAdditionalFee((IPaymentMethod)this, this.OrderTotalCalculationService, cart, new Decimal(0), false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return num;
        }

        public override void GetConfigurationRoute(out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        {
            actionName = StringExtensions.FormatInvariant("{0}Configure", new object[1]
      {
        (object) this.GetActionPrefix()
      });
            controllerName = "PayU";
            routeValues = new RouteValueDictionary()
      {
        {
          "area",
          (object) "Antargyan.PayU"
        }
      };
        }

        public override void GetPaymentInfoRoute(out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        {
            actionName = StringExtensions.FormatInvariant("{0}PaymentInfo", new object[1]
      {
        (object) this.GetActionPrefix()
      });
            controllerName = "PayU";
            routeValues = new RouteValueDictionary()
      {
        {
          "area",
          (object) "Antargyan.PayU"
        }
      };
        }

        public override ProcessPaymentResult ProcessPayment(ProcessPaymentRequest processPaymentRequest)
        {
            ProcessPaymentResult processPaymentResult = new ProcessPaymentResult();
            processPaymentResult.NewPaymentStatus=((PaymentStatus)10);
            return processPaymentResult;
        }
    }
}
