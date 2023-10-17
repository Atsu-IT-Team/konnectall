

// Nop.Plugin.Payments.MyFatoorah, Version=1.0.0.2, Culture=neutral, PublicKeyToken=null
// Nop.Plugin.Payments.MyFatoorah.MyFatoorahPaymentProcessor
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Directory;
using Nop.Core.Domain.Localization;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Payments;
using Nop.Plugin.Payments.MyFatoorah;
using Nop.Plugin.Payments.MyFatoorah.Controllers;
using Nop.Plugin.Payments.MyFatoorah.Models;
using Nop.Plugin.Payments.MyFatoorah.Models.Enums;
using Nop.Plugin.Payments.MyFatoorah.Services;
using Nop.Services.Catalog;
using Nop.Services.Common;
using Nop.Services.Configuration;
using Nop.Services.Customers;
using Nop.Services.Directory;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Messages;
using Nop.Services.Orders;
using Nop.Services.Payments;
using Nop.Services.Plugins;
using Nop.Services.Tax;

public class MyFatoorahPaymentProcessor : BasePlugin, IPaymentMethod, IPlugin
{
    private readonly IOrderTotalCalculationService _orderTotalCalculationService;

    private readonly CurrencySettings _currencySettings;

    private readonly ITaxService _taxService;

    private readonly ICheckoutAttributeParser _checkoutAttributeParser;

    private readonly ICountryService _countryService;

    private readonly IGenericAttributeService _genericAttributeService;

    private readonly ICurrencyService _currencyService;

    private readonly IAddressService _addressService;

    private readonly ILocalizationService _localizationService;

    private readonly ISettingService _settingService;

    private readonly IWebHelper _webHelper;

    private readonly MyFatoorahPaymentSettings _myFatoorahPaymentSettings;

    private readonly IWorkContext _workContext;

    private readonly ILogger _logger;

    private readonly IHttpContextAccessor _httpContextAccessor;

    private readonly IPaymentService _paymentService;

    private readonly IOrderService _orderService;

    private readonly IProductService _productService;

    private readonly MyFatoorahHttpClient _myFatoorahHttpClient;

    private readonly ILanguageService _languageService;

    private readonly INotificationService _notificationService;

    private readonly OrderSettings _orderSettings;

    private readonly IOrderProcessingService _orderProcessingService;

    private readonly IGiftCardService _giftCardService;

    private readonly ICustomerService _customerService;

    private readonly IShoppingCartService _shoppingCartService;

    private readonly IRewardPointService _rewardPointService;

    public bool SupportCapture => false;

    public bool SupportPartiallyRefund => false;

    public bool SupportRefund => false;

    public bool SupportVoid => false;

    public RecurringPaymentType RecurringPaymentType => (RecurringPaymentType)0;

    public PaymentMethodType PaymentMethodType => (PaymentMethodType)15;

    public bool SkipPaymentInfo => false;

    public MyFatoorahPaymentProcessor(CurrencySettings currencySettings, ICheckoutAttributeParser checkoutAttributeParser, ICurrencyService currencyService, IGenericAttributeService genericAttributeService, ILocalizationService localizationService, IOrderTotalCalculationService orderTotalCalculationService, ISettingService settingService, ITaxService taxService, IWebHelper webHelper, MyFatoorahPaymentSettings myFatoorahPaymentSettings, IWorkContext workContext, ILogger logger, IHttpContextAccessor httpContextAccessor, IPaymentService paymentService, IOrderService orderService, IAddressService addressService, IProductService productService, OrderSettings orderSettings, MyFatoorahHttpClient myFatoorahHttpClient, ICountryService countryService, ILanguageService languageService, INotificationService notificationService, IOrderProcessingService orderProcessingService, IGiftCardService giftCardService, ICustomerService customerService, IShoppingCartService shoppingCartService, IRewardPointService rewardPointService)
    {
        _currencySettings = currencySettings;
        _checkoutAttributeParser = checkoutAttributeParser;
        _currencyService = currencyService;
        _genericAttributeService = genericAttributeService;
        _localizationService = localizationService;
        _orderTotalCalculationService = orderTotalCalculationService;
        _settingService = settingService;
        _taxService = taxService;
        _webHelper = webHelper;
        _myFatoorahPaymentSettings = myFatoorahPaymentSettings;
        _workContext = workContext;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
        _paymentService = paymentService;
        _orderService = orderService;
        _addressService = addressService;
        _productService = productService;
        _myFatoorahHttpClient = myFatoorahHttpClient;
        _countryService = countryService;
        _languageService = languageService;
        _notificationService = notificationService;
        _orderSettings = orderSettings;
        _orderProcessingService = orderProcessingService;
        _giftCardService = giftCardService;
        _customerService = customerService;
        _shoppingCartService = shoppingCartService;
        _rewardPointService = rewardPointService;
    }

    public Task<ProcessPaymentResult> ProcessPaymentAsync(ProcessPaymentRequest processPaymentRequest)
    {
        //IL_0000: Unknown result type (might be due to invalid IL or missing references)
        //IL_0005: Unknown result type (might be due to invalid IL or missing references)
        //IL_000e: Expected O, but got Unknown
        ProcessPaymentResult result = new ProcessPaymentResult
        {
            NewPaymentStatus = (PaymentStatus)10
        };
        if (string.IsNullOrEmpty(_myFatoorahPaymentSettings.APIToken))
        {
            string errorMsg = "Invalid API Token, Please contact MyFatoorah customer support";
            _notificationService.ErrorNotification(errorMsg, true);
            result.AddError(errorMsg);
        }
        return Task.FromResult<ProcessPaymentResult>(result);
    }

    public async Task PostProcessPaymentAsync(PostProcessPaymentRequest postProcessPaymentRequest)
    {
        string orderDetailPage = $"{_webHelper.GetStoreLocation((bool?)false)}orderdetails/{((BaseEntity)postProcessPaymentRequest.Order).Id}";
        _orderSettings.DisableOrderCompletedPage = false;
        SendPaymentRequest invoiceRequest = await BuildInvoiceRequestAsync(postProcessPaymentRequest);
        string mfLog = "Request JSON: " + JsonConvert.SerializeObject(invoiceRequest);
        await _orderService.InsertOrderNoteAsync(new OrderNote
        {
            OrderId = ((BaseEntity)postProcessPaymentRequest.Order).Id,
            Note = mfLog,
            DisplayToCustomer = false,
            CreatedOnUtc = DateTime.UtcNow
        });
        SendPaymentResponse createInvoiceResult = _myFatoorahHttpClient.CreateInvoiceAsync(invoiceRequest).Result;
        mfLog = $"Status: {createInvoiceResult?.IsSuccess}, Message : {createInvoiceResult?.MessageSummary}";
        await _orderService.InsertOrderNoteAsync(new OrderNote
        {
            OrderId = ((BaseEntity)postProcessPaymentRequest.Order).Id,
            Note = mfLog,
            DisplayToCustomer = false,
            CreatedOnUtc = DateTime.UtcNow
        });
        if (!createInvoiceResult.IsSuccess)
        {
            _notificationService.ErrorNotification(createInvoiceResult.MessageSummary, true);
            _orderSettings.DisableOrderCompletedPage = true;
            _httpContextAccessor.HttpContext.Response.Redirect(orderDetailPage);
        }
        else
        {
            postProcessPaymentRequest.Order.CustomOrderNumber = Convert.ToString(createInvoiceResult.Data.InvoiceId);
            await _orderService.UpdateOrderAsync(postProcessPaymentRequest.Order);
            _httpContextAccessor.HttpContext.Response.Redirect(createInvoiceResult.Data.InvoiceURL);
        }
    }

    private async Task<SendPaymentRequest> BuildInvoiceRequestAsync(PostProcessPaymentRequest postProcessPaymentRequest)
    {
        string returnUrl = _webHelper.GetStoreLocation((bool?)false).Replace("localhost", "127.0.0.0") + "Plugins/PaymentMyFatoorah/PDTHandler";
        string cancelReturnUrl = _webHelper.GetStoreLocation((bool?)false).Replace("localhost", "127.0.0.0") + "Plugins/PaymentMyFatoorah/CancelOrder";
        List<Invoiceitem> invoiceItems = new List<Invoiceitem>();
        _ = (await _workContext.GetCurrentCustomerAsync()).Active;
        Order currentOrder = postProcessPaymentRequest.Order;
        decimal currencyRate = currentOrder.CurrencyRate;
        foreach (OrderItem item in await _orderService.GetOrderItemsAsync(((BaseEntity)currentOrder).Id, (bool?)null, (bool?)null, 0))
        {
            decimal unitPriceExclTaxRounded = Math.Round(item.UnitPriceExclTax, 3);
            Product product = await _productService.GetProductByIdAsync(item.ProductId);
            invoiceItems.Add(new Invoiceitem
            {
                ItemName = product.Name,
                Quantity = item.Quantity,
                UnitPrice = unitPriceExclTaxRounded * currencyRate
            });
        }
        decimal totalSubtotalExcludeExtra = invoiceItems.Sum((Invoiceitem x) => (decimal)x.Quantity * x.UnitPrice);
        decimal orderSubTotal = currentOrder.OrderSubtotalExclTax * currencyRate;
        await _customerService.GetCustomerByIdAsync(currentOrder.CustomerId);
        Dictionary<string, decimal> otherItems = new Dictionary<string, decimal>();
        if (totalSubtotalExcludeExtra != orderSubTotal)
        {
            otherItems.Add(currentOrder.CheckoutAttributeDescription, orderSubTotal - totalSubtotalExcludeExtra);
        }
        if (currentOrder.OrderShippingExclTax > 0m)
        {
            otherItems.Add("Shipping " + currentOrder.ShippingMethod, currentOrder.OrderShippingExclTax);
        }
        if (currentOrder.OrderTax > 0m)
        {
            otherItems.Add("Tax", currentOrder.OrderTax);
        }
        if (currentOrder.OrderDiscount > 0m)
        {
            otherItems.Add("Discount", -currentOrder.OrderDiscount);
        }
        if (currentOrder.OrderSubTotalDiscountInclTax > 0m)
        {
            otherItems.Add("Sub Total Discount", -currentOrder.OrderSubTotalDiscountInclTax);
        }
        if (currentOrder.PaymentMethodAdditionalFeeExclTax > 0m)
        {
            otherItems.Add("Additional Fee", currentOrder.PaymentMethodAdditionalFeeExclTax);
        }
        new List<AppliedGiftCard>();
        IList<GiftCardUsageHistory> giftCards = await _giftCardService.GetGiftCardUsageHistoryAsync(currentOrder);
        if (giftCards != null)
        {
            foreach (GiftCardUsageHistory gc in giftCards)
            {
                otherItems.Add("GiftCard (" + (await _giftCardService.GetGiftCardByIdAsync(gc.GiftCardId)).GiftCardCouponCode + ") ", -gc.UsedValue);
            }
        }
        IRewardPointService rewardPointService = _rewardPointService;
        Guid? guid = currentOrder.OrderGuid;
        IPagedList<RewardPointsHistory> rewardPoints = await rewardPointService.GetRewardPointsHistoryAsync(0, (int?)null, false, guid, 0, int.MaxValue);
        if (rewardPoints != null)
        {
            foreach (RewardPointsHistory rp in (IEnumerable<RewardPointsHistory>)rewardPoints)
            {
                otherItems.Add($"Reward Points ({rp.Points}) ", -rp.UsedAmount);
            }
        }
        if (otherItems.Any())
        {
            invoiceItems.AddRange(otherItems.Select((KeyValuePair<string, decimal> x) => new Invoiceitem
            {
                ItemName = x.Key,
                Quantity = 1,
                UnitPrice = x.Value * currencyRate
            }));
        }
        Address billingAddress = await _addressService.GetAddressByIdAsync(currentOrder.BillingAddressId);
        Language orderLanguage = await _languageService.GetLanguageByIdAsync(currentOrder.CustomerLanguageId);
        Currency orderCurrency = await _currencyService.GetCurrencyByCodeAsync(currentOrder.CustomerCurrencyCode);
        decimal invoiceValue = Math.Round(invoiceItems.Sum((Invoiceitem x) => (decimal)x.Quantity * x.UnitPrice), 3);
        SendPaymentRequest obj = new SendPaymentRequest
        {
            CustomerName = billingAddress.FirstName + " " + billingAddress.LastName
        };
        string phoneNumber = billingAddress.PhoneNumber;
        obj.CustomerMobile = ((phoneNumber != null && phoneNumber.Length > 10) ? "0" : billingAddress.PhoneNumber);
        obj.CustomerEmail = billingAddress.Email;
        obj.InvoiceItems = invoiceItems;
        obj.InvoiceValue = invoiceValue;
        obj.CustomerReference = ((BaseEntity)currentOrder).Id.ToString();
        obj.ExpiryDate = DateTime.Now.AddHours(1.0);
        obj.Language = (orderLanguage.Rtl ? "AR" : "EN");
        obj.UserDefinedField = ((BaseEntity)currentOrder).Id.ToString();
        obj.DisplayCurrencyIso = ((_myFatoorahPaymentSettings.DisplayCurrencyIsoAlpha == string.Empty) ? "KWD" : _myFatoorahPaymentSettings.DisplayCurrencyIsoAlpha);
        obj.CallBackUrl = returnUrl;
        obj.ErrorUrl = cancelReturnUrl;
        SendPaymentRequest invoiceRequest = obj;
        invoiceRequest.DisplayCurrencyIso = orderCurrency.CurrencyCode;
        if (_myFatoorahPaymentSettings.SendInvoiceOption.HasValue)
        {
            SendPaymentRequest sendPaymentRequest = invoiceRequest;
            SendInvoiceOptionEnum value = (SendInvoiceOptionEnum)_myFatoorahPaymentSettings.SendInvoiceOption.Value;
            string notificationOption = default(string);
            switch (value)
            {
                case SendInvoiceOptionEnum.SMS:
                    notificationOption = "SMS";
                    break;
                case SendInvoiceOptionEnum.Email:
                    notificationOption = "EML";
                    break;
                case SendInvoiceOptionEnum.SMS_AND_EMAIL:
                    notificationOption = "ALL";
                    break;
                case SendInvoiceOptionEnum.NONE:
                    notificationOption = "LNK";
                    break;
                default:
                    //global::< PrivateImplementationDetails >.ThrowSwitchExpressionException(value);
                    break;
            }
            sendPaymentRequest.NotificationOption = notificationOption;
        }
        invoiceRequest.CustomerMobile = null;
        invoiceRequest.MobileCountryCode = null;
        invoiceRequest.NotificationOption = "LNK";
        return invoiceRequest;
    }

    public Task<bool> HidePaymentMethodAsync(IList<ShoppingCartItem> cart)
    {
        return Task.FromResult(result: false);
    }

    public async Task<decimal> GetAdditionalHandlingFeeAsync(IList<ShoppingCartItem> cart)
    {
        return default(decimal);
    }

    public Task<CapturePaymentResult> CaptureAsync(CapturePaymentRequest capturePaymentRequest)
    {
        //IL_0000: Unknown result type (might be due to invalid IL or missing references)
        //IL_0006: Expected O, but got Unknown
        CapturePaymentResult val = new CapturePaymentResult();
        val.Errors = new string[1] { "Capture method not supported" };
        return Task.FromResult<CapturePaymentResult>(val);
    }

    public Task<RefundPaymentResult> RefundAsync(RefundPaymentRequest refundPaymentRequest)
    {
        //IL_0000: Unknown result type (might be due to invalid IL or missing references)
        //IL_0006: Expected O, but got Unknown
        RefundPaymentResult val = new RefundPaymentResult();
        val.Errors = new string[1] { "Refund method not supported" };
        return Task.FromResult<RefundPaymentResult>(val);
    }

    public Task<VoidPaymentResult> VoidAsync(VoidPaymentRequest voidPaymentRequest)
    {
        //IL_0000: Unknown result type (might be due to invalid IL or missing references)
        //IL_0006: Expected O, but got Unknown
        VoidPaymentResult val = new VoidPaymentResult();
        val.Errors = new string[1] { "Void method not supported" };
        return Task.FromResult<VoidPaymentResult>(val);
    }

    public Task<ProcessPaymentResult> ProcessRecurringPaymentAsync(ProcessPaymentRequest processPaymentRequest)
    {
        //IL_0000: Unknown result type (might be due to invalid IL or missing references)
        //IL_0006: Expected O, but got Unknown
        ProcessPaymentResult val = new ProcessPaymentResult();
        val.Errors = new string[1] { "Recurring payment not supported" };
        return Task.FromResult<ProcessPaymentResult>(val);
    }

    public Task<CancelRecurringPaymentResult> CancelRecurringPaymentAsync(CancelRecurringPaymentRequest cancelPaymentRequest)
    {
        //IL_0000: Unknown result type (might be due to invalid IL or missing references)
        //IL_0006: Expected O, but got Unknown
        CancelRecurringPaymentResult val = new CancelRecurringPaymentResult();
        val.Errors = new string[1] { "Recurring payment not supported" };
        return Task.FromResult<CancelRecurringPaymentResult>(val);
    }

    public Task<bool> CanRePostProcessPaymentAsync(Order order)
    {
        if (order == null)
        {
            throw new ArgumentNullException("order");
        }
        return Task.FromResult(result: true);
    }

    public void GetConfigurationRoute(out string actionName, out string controllerName, out RouteValueDictionary routeValues)
    {
        actionName = "Configure";
        controllerName = "PaymentMyFatoorah";
        routeValues = new RouteValueDictionary
        {
            { "Namespaces", "Nop.Plugin.Payments.MyFatoorah.Controllers" },
            { "area", null }
        };
    }

    public void GetPaymentInfoRoute(out string actionName, out string controllerName, out RouteValueDictionary routeValues)
    {
        actionName = "PaymentInfo";
        controllerName = "PaymentMyFatoorah";
        routeValues = new RouteValueDictionary
        {
            { "Namespaces", "Nop.Plugin.Payments.MyFatoorah.Controllers" },
            { "area", null }
        };
    }

    public Type GetControllerType()
    {
        return typeof(PaymentMyFatoorahController);
    }

    public override async Task InstallAsync()
    {
        MyFatoorahPaymentSettings settings = new MyFatoorahPaymentSettings
        {
            UseSandbox = true,
            ReturnFromMyFatoorahWithoutPaymentRedirectsToOrderDetailsPage = true
        };
        await _settingService.SaveSettingAsync<MyFatoorahPaymentSettings>(settings, 0);
        await _localizationService.AddOrUpdateLocaleResourceAsync((IDictionary<string, string>)new Dictionary<string, string>
        {
            ["Plugins.Payments.MyFatoorah.Fields.AdditionalFee"] = "Additional fee",
            ["Plugins.Payments.MyFatoorah.Fields.AdditionalFee.Hint"] = "Enter additional fee to charge your customers.",
            ["Plugins.Payments.MyFatoorah.Fields.AdditionalFeePercentage"] = "Additional fee. Use percentage",
            ["Plugins.Payments.MyFatoorah.Fields.AdditionalFeePercentage.Hint"] = "Determines whether to apply a percentage additional fee to the order total. If not enabled, a fixed value is used.",
            ["Plugins.Payments.MyFatoorah.Fields.APIURL"] = "Enter API Url",
            ["Plugins.Payments.MyFatoorah.Fields.APIURL.Hint"] = "API Url",
            ["Plugins.Payments.MyFatoorah.Fields.APIToken"] = "API Token",
            ["Plugins.Payments.MyFatoorah.Fields.APIToken.Hint"] = "API Token",
            ["Plugins.Payments.MyFatoorah.Fields.ReturnURL"] = "Return URL",
            ["Plugins.Payments.MyFatoorah.Fields.ReturnURL.Hint"] = "Return URL",
            ["Plugins.Payments.MyFatoorah.Fields.ErrorURL"] = "Error URL",
            ["Plugins.Payments.MyFatoorah.Fields.ErrorURL.Hint"] = "Error URL",
            ["Plugins.Payments.MyFatoorah.Fields.DisplayCurrencyIsoAlpha"] = "Display Currency",
            ["Plugins.Payments.MyFatoorah.Fields.DisplayCurrencyIsoAlpha.Hint"] = "Display Currency",
            ["Plugins.Payments.MyFatoorah.Fields.PaymentAcknowledgement"] = "Payment  Acknowledgement",
            ["Plugins.Payments.MyFatoorah.Fields.PaymentAcknowledgement.Hint"] = "Payment Acknowledgement will send to customer by Email or SMS or both",
            ["Plugins.Payments.MyFatoorah.Fields.RedirectionTip"] = "You will be redirected to MyFatoorah site to complete the order.",
            ["Plugins.Payments.MyFatoorah.Fields.ReturnFromMyFatoorahWithoutPaymentRedirectsToOrderDetailsPage"] = "Return to order details page",
            ["Plugins.Payments.MyFatoorah.Fields.ReturnFromMyFatoorahWithoutPaymentRedirectsToOrderDetailsPage.Hint"] = "Enable if a customer should be redirected to the order details page when he clicks \"return to store\" link on MyFatoorah site WITHOUT completing a payment",
            ["Plugins.Payments.MyFatoorah.Fields.UseSandbox"] = "Use Sandbox",
            ["Plugins.Payments.MyFatoorah.Fields.UseSandbox.Hint"] = "Check to enable Sandbox (testing environment).",
            ["Plugins.Payments.MyFatoorah.PaymentMethodDescription"] = "Pay by MyFatoorah",
            ["Plugins.Payments.MyFatoorah.RoundingWarning"] = "It looks like you have \"ShoppingCartSettings.RoundPricesDuringCalculation\" setting disabled. Keep in mind that this can lead to a discrepancy of the order total amount, as MyFatoorah only rounds to two decimals.",
            ["Plugins.Payments.MyFatoorah.Instructions"] = "<p><b> If you're using this gateway ensure that your primary store currency is supported by MyFatoorah.</b><br /><br />",
            ["Plugins.Payments.MyFatoorah.Fields.Country"] = "Store Country",
            ["Plugins.Payments.MyFatoorah.Fields.Country.Hint"] = "Store Country"
        }, (int?)null);
        // await <> n__0();
    }

    public override async Task UninstallAsync()
    {
        await _settingService.DeleteSettingAsync<MyFatoorahPaymentSettings>();
        await _localizationService.DeleteLocaleResourcesAsync("Plugins.Payments.MyFatoorah", (int?)null);
        // await <> n__1();
    }

    public Task<IList<string>> ValidatePaymentFormAsync(IFormCollection form)
    {
        return Task.FromResult((IList<string>)new List<string>());
    }

    public Task<ProcessPaymentRequest> GetPaymentInfoAsync(IFormCollection form)
    {
        //IL_0000: Unknown result type (might be due to invalid IL or missing references)
        //IL_000a: Expected O, but got Unknown
        return Task.FromResult<ProcessPaymentRequest>(new ProcessPaymentRequest());
    }

    public string GetPublicViewComponentName()
    {
        return "PaymentMyFatoorah";
    }

    public override string GetConfigurationPageUrl()
    {
        return _webHelper.GetStoreLocation((bool?)null) + "Admin/PaymentMyFatoorah/Configure";
    }

    public async Task<string> GetPaymentMethodDescriptionAsync()
    {
        return await _localizationService.GetResourceAsync("Plugins.Payments.MyFatoorah.PaymentMethodDescription");
    }

    // [CompilerGenerated]
    // [DebuggerHidden]
    // private Task<> n__0()
    // {
    //     return ((BasePlugin)this).InstallAsync();
    // }

    // [CompilerGenerated]
    // [DebuggerHidden]
    // private Task<> n__1()
    // {
    //     return ((BasePlugin)this).UninstallAsync();
    // }
}
