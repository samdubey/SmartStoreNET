using Antargyan.PayU.Settings;
using SmartStore.Core.Domain.Localization;
using SmartStore.Core.Plugins;
using SmartStore.Services;
using SmartStore.Services.Configuration;
using SmartStore.Services.Localization;
using System;

namespace Antargyan.PayU
{
    public class Plugin : BasePlugin
    {
        private readonly ICommonServices _services;

        public Plugin(ICommonServices services)
            : base()
        {
            this._services = services;
        }
        public override void Install()
        {
            ISettingService settings = this._services.Settings;
            ILocalizationService localization = this._services.Localization;
            settings.SaveSetting<PayUSettings>(new PayUSettings()
            {
                Key = "",
                Salt = "",
                MerchantParam = "",
                PayUri = "https://test.payu.in/_payment",
                AdditionalFee = new Decimal(0),
                IsPayUBiz = true
            }, 0);
            ILocalizationService ilocalizationService1 = localization;
            LocaleStringResource localeStringResource1 = new LocaleStringResource()
            {
                ResourceName = "Plugins.Payments.PayU.RedirectionTip",
                ResourceValue = "You will be redirected to PayU site to complete the order.",
                LanguageId=_services.WorkContext.WorkingLanguage.Id
            };
            LocaleStringResource localeStringResource2 = localeStringResource1;
            ilocalizationService1.InsertLocaleStringResource(localeStringResource2);
            ILocalizationService ilocalizationService2 = localization;
            LocaleStringResource localeStringResource3 = new LocaleStringResource();
            localeStringResource3.ResourceName = ("Plugins.Payments.PayU.MerchantId");
            localeStringResource3.ResourceValue = ("Merchant ID");
            localeStringResource3.LanguageId = _services.WorkContext.WorkingLanguage.Id;
            
            LocaleStringResource localeStringResource4 = localeStringResource3;
            ilocalizationService2.InsertLocaleStringResource(localeStringResource4);
            ILocalizationService ilocalizationService3 = localization;
            LocaleStringResource localeStringResource5 = new LocaleStringResource();
            localeStringResource5.ResourceName = ("Plugins.Payments.PayU.MerchantId.Hint");
            localeStringResource5.ResourceValue = ("Enter merchant ID.");
            localeStringResource5.LanguageId = _services.WorkContext.WorkingLanguage.Id;

            LocaleStringResource localeStringResource6 = localeStringResource5;
            ilocalizationService3.InsertLocaleStringResource(localeStringResource6);
            ILocalizationService ilocalizationService4 = localization;
            LocaleStringResource localeStringResource7 = new LocaleStringResource();
            localeStringResource7.ResourceName = ("Plugins.Payments.PayU.Key");
            localeStringResource7.ResourceValue = ("Working Key");
            localeStringResource7.LanguageId = _services.WorkContext.WorkingLanguage.Id;
            LocaleStringResource localeStringResource8 = localeStringResource7;
            ilocalizationService4.InsertLocaleStringResource(localeStringResource8);
            ILocalizationService ilocalizationService5 = localization;
            LocaleStringResource localeStringResource9 = new LocaleStringResource();
            localeStringResource9.ResourceName = ("Plugins.Payments.PayU.Key.Hint");
            localeStringResource9.ResourceValue = ("Enter working key.");
            localeStringResource9.LanguageId = _services.WorkContext.WorkingLanguage.Id;
            LocaleStringResource localeStringResource10 = localeStringResource9;
            ilocalizationService5.InsertLocaleStringResource(localeStringResource10);
            ILocalizationService ilocalizationService6 = localization;
            LocaleStringResource localeStringResource11 = new LocaleStringResource();
            localeStringResource11.ResourceName = ("Plugins.Payments.PayU.MerchantParam");
            localeStringResource11.ResourceValue = ("Merchant Param");
            localeStringResource11.LanguageId = _services.WorkContext.WorkingLanguage.Id;
            LocaleStringResource localeStringResource12 = localeStringResource11;
            ilocalizationService6.InsertLocaleStringResource(localeStringResource12);
            ILocalizationService ilocalizationService7 = localization;
            LocaleStringResource localeStringResource13 = new LocaleStringResource();
            localeStringResource13.ResourceName = ("Plugins.Payments.PayU.MerchantParam.Hint");
            localeStringResource13.ResourceValue = ("Enter merchant param.");
            localeStringResource13.LanguageId = _services.WorkContext.WorkingLanguage.Id;
            LocaleStringResource localeStringResource14 = localeStringResource13;
            ilocalizationService7.InsertLocaleStringResource(localeStringResource14);
            ILocalizationService ilocalizationService8 = localization;
            LocaleStringResource localeStringResource15 = new LocaleStringResource();
            localeStringResource15.ResourceName = ("Plugins.Payments.PayU.PayUri");
            localeStringResource15.ResourceValue = ("Pay URI");
            localeStringResource15.LanguageId = _services.WorkContext.WorkingLanguage.Id;
            LocaleStringResource localeStringResource16 = localeStringResource15;
            ilocalizationService8.InsertLocaleStringResource(localeStringResource16);
            ILocalizationService ilocalizationService9 = localization;
            LocaleStringResource localeStringResource17 = new LocaleStringResource();
            localeStringResource17.ResourceName = ("Plugins.Payments.PayU.PayUri.Hint");
            localeStringResource17.ResourceValue = ("Enter Pay URI.");
            localeStringResource17.LanguageId = _services.WorkContext.WorkingLanguage.Id;
            LocaleStringResource localeStringResource18 = localeStringResource17;
            ilocalizationService9.InsertLocaleStringResource(localeStringResource18);
            ILocalizationService ilocalizationService10 = localization;
            LocaleStringResource localeStringResource19 = new LocaleStringResource();
            localeStringResource19.ResourceName = ("Plugins.Payments.PayU.AdditionalFee");
            localeStringResource19.ResourceValue = ("Additional fee");
            localeStringResource19.LanguageId = _services.WorkContext.WorkingLanguage.Id;
            LocaleStringResource localeStringResource20 = localeStringResource19;
            ilocalizationService10.InsertLocaleStringResource(localeStringResource20);
            ILocalizationService ilocalizationService11 = localization;
            LocaleStringResource localeStringResource21 = new LocaleStringResource();
            localeStringResource21.ResourceName = ("Plugins.Payments.PayU.AdditionalFee.Hint");
            localeStringResource21.ResourceValue = ("Enter additional fee to charge your customers.");
            localeStringResource21.LanguageId = _services.WorkContext.WorkingLanguage.Id;
            LocaleStringResource localeStringResource22 = localeStringResource21;
            ilocalizationService11.InsertLocaleStringResource(localeStringResource22);
            base.Install();
        }

        public override void Uninstall()
        {
            ISettingService settings = _services.Settings;
            ILocalizationService localization = _services.Localization;
            localization.DeleteLocaleStringResources(this.PluginDescriptor.ResourceRootKey, true);
            localization.DeleteLocaleStringResources("Plugins.Payments.PayU.RedirectionTip", true);
            localization.DeleteLocaleStringResources("Plugins.Payments.PayU.MerchantId", true);
            localization.DeleteLocaleStringResources("Plugins.Payments.PayU.MerchantId.Hint", true);
            localization.DeleteLocaleStringResources("Plugins.Payments.PayU.Key", true);
            localization.DeleteLocaleStringResources("Plugins.Payments.PayU.Key.Hint", true);
            localization.DeleteLocaleStringResources("Plugins.Payments.PayU.MerchantParam", true);
            localization.DeleteLocaleStringResources("Plugins.Payments.PayU.MerchantParam.Hint", true);
            localization.DeleteLocaleStringResources("Plugins.Payments.PayU.PayUri", true);
            localization.DeleteLocaleStringResources("Plugins.Payments.PayU.PayUri.Hint", true);
            localization.DeleteLocaleStringResources("Plugins.Payments.PayU.AdditionalFee", true);
            localization.DeleteLocaleStringResources("Plugins.Payments.PayU.AdditionalFee.Hint", true);
            settings.DeleteSetting<PayUSettings>();
            base.Uninstall();
        }
    }
}