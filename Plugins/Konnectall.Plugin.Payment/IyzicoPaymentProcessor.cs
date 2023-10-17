using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Konnectall.Plugin.Payment.Validators;
using Microsoft.AspNetCore.Http;
using Nop.Core;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Orders;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Payments;
using Nop.Services.Plugins;
using Konnectall.Plugin.Payment.Models;
namespace Konnectall.Plugin.Payment
{
    public class IyzicoPaymentProcessor : BasePlugin, IPaymentMethod
    {
        private readonly IWebHelper _webHelper;
        private readonly ISettingService _settingService;
        private readonly ILocalizationService _localizationService;
        public IyzicoPaymentProcessor(ISettingService settingService, IWebHelper webHelper, ILocalizationService localizationService)
        {
            _settingService = settingService;
            _webHelper = webHelper;
            _localizationService = localizationService;

        }
        public override async Task InstallAsync()
        {
            //settings
            var settings = new IyzicoSettings
            {
                ApiKey = "",
                SecretKey = "",
                CallbackUrl = "",
                Uri = "",
                CustomerIdentityAttributeKey = ""
            };
            await _settingService.SaveSettingAsync(settings);
            // await base.InstallAsync();
            await _localizationService.AddOrUpdateLocaleResourceAsync("Nop.Plugin.Payments.Iyzico.Configuration.Fields.ApiKey", "Api Key");
            await _localizationService.AddOrUpdateLocaleResourceAsync("Nop.Plugin.Payments.Iyzico.Configuration.Fields.ApiKey.Hint", "Api Key");
            await _localizationService.AddOrUpdateLocaleResourceAsync("Nop.Plugin.Payments.Iyzico.Configuration.Fields.SecretKey", "Secret Key");
            await _localizationService.AddOrUpdateLocaleResourceAsync("Nop.Plugin.Payments.Iyzico.Configuration.Fields.SecretKey.Hint", "Secret Key");
            await _localizationService.AddOrUpdateLocaleResourceAsync("Nop.Plugin.Payments.Iyzico.Configuration.Fields.Uri", "Uri");
            await _localizationService.AddOrUpdateLocaleResourceAsync("Nop.Plugin.Payments.Iyzico.Configuration.Fields.Uri. Hint", "Uri");
            await _localizationService.AddOrUpdateLocaleResourceAsync("Nop.Plugin.Payments.Iyzico.Configuration.Fields.CallbackUrl", "Callback Url");
            await _localizationService.AddOrUpdateLocaleResourceAsync("Nop.Plugin.Payments.Iyzico.Configuration.Fields.CallbackUrl.Hint", "Callback Url");
            await _localizationService.AddOrUpdateLocaleResourceAsync("Nop.Plugin.Payments.Iyzico.Configuration.Fields.CustomerIdentityNumberAttributeKey", "Customer Identity Number Attribute Key");
            await _localizationService.AddOrUpdateLocaleResourceAsync("Nop.Plugin.Payments.Iyzico.Configuration.Fields.CustomerIdentityNumberAttributeKey.Hint", "Customer Identity Number Attribute Key");
            await _localizationService.AddOrUpdateLocaleResourceAsync("Nop.Plugin.Payments.Iyzico.Configuration.General", "General Setting");
        }
        public override async Task UninstallAsync()
        {
            await _settingService.DeleteSettingAsync<IyzicoSettings>();
            await _localizationService.DeleteLocaleResourcesAsync("Nop.Plugin.Payments.Iyzico");
        }
        public bool SupportCapture => false;

        public bool SupportPartiallyRefund => false;

        public bool SupportRefund => false;

        public bool SupportVoid => false;

        public RecurringPaymentType RecurringPaymentType => RecurringPaymentType.Manual;

        public PaymentMethodType PaymentMethodType => PaymentMethodType.Standard;

        public bool SkipPaymentInfo => false;

        public Task<CancelRecurringPaymentResult> CancelRecurringPaymentAsync(CancelRecurringPaymentRequest cancelPaymentRequest)
        {
            return Task.FromResult(new CancelRecurringPaymentResult());
        }

        public Task<bool> CanRePostProcessPaymentAsync(Order order)
        {
            return Task.FromResult(false);
        }

        public Task<CapturePaymentResult> CaptureAsync(CapturePaymentRequest capturePaymentRequest)
        {
            return Task.FromResult(new CapturePaymentResult());
        }

        public Task<decimal> GetAdditionalHandlingFeeAsync(IList<ShoppingCartItem> cart)
        {
            return Task.FromResult(0m);
        }

        public Task<ProcessPaymentRequest> GetPaymentInfoAsync(IFormCollection form)
        {
            return Task.FromResult(new ProcessPaymentRequest());
        }

        public Task<string> GetPaymentMethodDescriptionAsync()
        {
            return Task.FromResult("Iyzico Payment Method description");
        }
        public override string GetConfigurationPageUrl()
        {
            /// TODO : Configuration Page url
            return $"{_webHelper.GetStoreLocation()}Admin/IyzicoConfiguration/Configure";
        }
        public string GetPublicViewComponentName()
        {
            return "IyzicoPublic";
        }

        public Task<bool> HidePaymentMethodAsync(IList<ShoppingCartItem> cart)
        {
            return Task.FromResult(false);
        }

        public Task PostProcessPaymentAsync(PostProcessPaymentRequest postProcessPaymentRequest)
        {
            return Task.CompletedTask;
        }

        public Task<ProcessPaymentResult> ProcessPaymentAsync(ProcessPaymentRequest processPaymentRequest)
        {
            return Task.FromResult(new ProcessPaymentResult());
        }

        public Task<ProcessPaymentResult> ProcessRecurringPaymentAsync(ProcessPaymentRequest processPaymentRequest)
        {
            return Task.FromResult(new ProcessPaymentResult());
        }

        public Task<RefundPaymentResult> RefundAsync(RefundPaymentRequest refundPaymentRequest)
        {
            return Task.FromResult(new RefundPaymentResult());
        }


        public async Task<IList<string>> ValidatePaymentFormAsync(IFormCollection form)
        {
            var errors = new List<string>();
            var validator = new IyzicoPublicInfoModelValidator(_localizationService);
            var model = new PaymentInfoModel
            {
                CardholderName = form[nameof(PaymentInfoModel.CardholderName)],
                CardNumber = form[nameof(PaymentInfoModel.CardNumber)],
                ExpireMonth = form[nameof(PaymentInfoModel.ExpireMonth)],
                ExpireYear = form[nameof(PaymentInfoModel.ExpireYear)],
                CardCode = form[nameof(PaymentInfoModel.CardCode)]
            };
            var validationResult = await validator.ValidateAsync(model);
            if (!validationResult.IsValid)
                errors.AddRange(validationResult.Errors.Select(e => e.ErrorMessage));
            return errors;
        }

        public Task<VoidPaymentResult> VoidAsync(VoidPaymentRequest voidPaymentRequest)
        {
            return Task.FromResult(new VoidPaymentResult());
        }
    }
}
