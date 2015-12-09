using SmartStore.Web.Framework;
using System;

namespace Antargyan.PayU.Settings
{
    public class PayUSettings : PaymentSettingsBase
    {
        [SmartResourceDisplayName("Plugins.Antargyan.PayU.MerchantId", "Key")]
        public string Key { get; set; }

        [SmartResourceDisplayName("Plugins.Antargyan.PayU.Key", "Salt")]
        public string Salt { get; set; }

        [SmartResourceDisplayName("Plugins.Antargyan.PayU.MerchantParam", "MerchantParam")]
        public string MerchantParam { get; set; }

        [SmartResourceDisplayName("Plugins.Antargyan.PayU.PayUri", "PayUri")]
        public string PayUri { get; set; }

        [SmartResourceDisplayName("Plugins.Antargyan.PayU.AdditionalFee", "AdditionalFee")]
        public Decimal AdditionalFee { get; set; }

        [SmartResourceDisplayName("Plugins.Antargyan.PayU.IsPayUBiz", "IsPayUBiz")]
        public bool IsPayUBiz { get; set; }
    }
}