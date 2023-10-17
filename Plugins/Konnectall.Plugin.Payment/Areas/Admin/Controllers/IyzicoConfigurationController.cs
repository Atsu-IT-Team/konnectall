using Autofac.Core;
using DocumentFormat.OpenXml.EMMA;
using Konnectall.Plugin.Payment.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using Nop.Core.Domain.Configuration;
using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Services.Common;
using Nop.Services.Configuration;
using Nop.Services.Customers;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Konnectall.Plugin.Payment.Areas.Admin.Controllers
{
    [Area(AreaNames.Admin)]
    [AutoValidateAntiforgeryToken]
    [ValidateIpAddress]
    [AuthorizeAdmin]
    [ValidateVendor]
    [SaveSelectedTab]
    public class IyzicoConfigurationController : BasePluginController
    {
        private readonly IWorkContext _workContext;
        private readonly ISettingService _settingService;
        private readonly ILocalizationService _localizationService;
        private readonly INotificationService _notificationService;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly ICustomerAttributeService _customerAttributeService;

        public IyzicoConfigurationController(
        IWorkContext workContext,
        ISettingService settingService,
        ILocalizationService localizationService,
        INotificationService notificationService,
        IGenericAttributeService genericAttributeService,
        ICustomerAttributeService customerAttributeService)
        {
            _workContext = workContext;
            _settingService = settingService;
            _localizationService = localizationService;
            _notificationService = notificationService;
            _genericAttributeService = genericAttributeService;
            _customerAttributeService = customerAttributeService;
        }

        public async Task<IActionResult> Configure()
        {
            var settings = await _settingService.LoadSettingAsync<IyzicoSettings>();
            var model = new ConfigurationModel
            {
                ApiKey = settings.ApiKey,
                SecretKey = settings.SecretKey,
                Uri = settings.Uri,
                CallbackUrl = settings.CallbackUrl,
                CustomerIdentityAttributeKey = settings.CustomerIdentityAttributeKey
            };
            return View("~/Plugins/Konnectall.plugin.Payment/Areas/Admin/Views/IyzicoConfiguration/Configure.cshtml", model);
        }
        [HttpPost]
        public async Task<IActionResult> Configure(ConfigurationModel model)
        {
            var settings = new IyzicoSettings
            {
                ApiKey = model.ApiKey,
                SecretKey = model.SecretKey,
                Uri = model.Uri,
                CallbackUrl = model.CallbackUrl,
                CustomerIdentityAttributeKey = model.CustomerIdentityAttributeKey
            };
            await _settingService.SaveSettingAsync(settings);
            await _settingService.ClearCacheAsync();
            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Plugins.Saved"));
            return await Configure();
        }
    }
}
