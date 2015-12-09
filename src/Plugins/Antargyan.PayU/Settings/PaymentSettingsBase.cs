using SmartStore.Core.Configuration;

namespace Antargyan.PayU.Settings
{
    public abstract class PaymentSettingsBase : ISettings
    {
        public string DescriptionText { get; set; }
    }
}
