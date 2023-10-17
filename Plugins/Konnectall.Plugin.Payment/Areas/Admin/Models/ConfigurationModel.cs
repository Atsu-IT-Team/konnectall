using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Konnectall.Plugin.Payment.Areas.Admin.Models
{
    public record ConfigurationModel : BaseNopModel, ISettingsModel
    {
        public int ActiveStoreScopeConfiguration { get; set; }
        [NopResourceDisplayName("Plugins.Payments.Iyzico.Configuration.Fields.ApiKey")]
        public string ApiKey { get; set; }
        [NopResourceDisplayName("Plugins.Payments.Iyzico.Configuration.Fields.SecretKey")]
        public string SecretKey { get; set; }
        [NopResourceDisplayName("Plugins.Payments.Iyzico.Configuration.Fields.Uri")]
        public string Uri { get; set; }
        [NopResourceDisplayName("Plugins.Payments.Iyzico.Configuration.Fields.CallbackUrl")] 
        public string CallbackUrl { get; set; }
        [NopResourceDisplayName("Plugins.Payments.Iyzico.Configuration.Fields.CustomerIdentityAttributeKey")]
        public string CustomerIdentityAttributeKey { get; set; }
    }
}
