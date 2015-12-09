using SmartStore.Web.Framework;
using SmartStore.Web.Framework.Mvc;
using System.Web.Mvc;

namespace Antargyan.PayU.Models
{
    public abstract class ConfigurationModelBase : ModelBase
    {
        [SmartResourceDisplayName("Plugins.Antargyan.PayU.DescriptionText", "DescriptionText")]
        [AllowHtml]
        public string DescriptionText { get; set; }
    }
}
