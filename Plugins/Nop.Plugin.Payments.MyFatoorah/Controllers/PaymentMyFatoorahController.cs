// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.Payments.MyFatoorah.Controllers.PaymentMyFatoorahController
// Assembly: Nop.Plugin.Payments.MyFatoorah, Version=1.0.0.2, Culture=neutral, PublicKeyToken=null
// MVID: 74EEC5F1-0E85-4A72-B69A-76B24BEA288E
// Assembly location: C:\Users\USER\Downloads\Nop.Plugin.Payments.MyFatoorah_main_4.5\Payments.MyFatoorah\Nop.Plugin.Payments.MyFatoorah.dll

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Payments;
using Nop.Plugin.Payments.MyFatoorah.Models;
using Nop.Plugin.Payments.MyFatoorah.Services;
using Nop.Services.Common;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Messages;
using Nop.Services.Orders;
using Nop.Services.Payments;
using Nop.Services.Plugins;
using Nop.Services.Security;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;


#nullable enable
namespace Nop.Plugin.Payments.MyFatoorah.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class PaymentMyFatoorahController : BasePaymentController
    {
        private readonly IActionContextAccessor _actionContextAccessor;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IOrderProcessingService _orderProcessingService;
        private readonly IOrderService _orderService;
        private readonly IPaymentPluginManager _paymentPluginManager;
        private readonly IPermissionService _permissionService;
        private readonly ILocalizationService _localizationService;
        private readonly ILogger _logger;
        private readonly INotificationService _notificationService;
        private readonly ISettingService _settingService;
        private readonly IStoreContext _storeContext;
        private readonly IWebHelper _webHelper;
        private readonly IWorkContext _workContext;
        private readonly IUrlHelperFactory _urlHelperFactory;
        private readonly ShoppingCartSettings _shoppingCartSettings;
        private readonly MyFatoorahPaymentSettings _myFatoorahPaymentSettings;
        private readonly MyFatoorahHttpClient _myFatoorahHttpClient;

        public PaymentMyFatoorahController(IActionContextAccessor actionContextAccessor,
          IGenericAttributeService genericAttributeService,
          IOrderProcessingService orderProcessingService,
          IOrderService orderService,
          IPaymentPluginManager paymentPluginManager,
          IPermissionService permissionService,
          ILocalizationService localizationService,
          ILogger logger,
          INotificationService notificationService,
          ISettingService settingService,
          IStoreContext storeContext,
          IWebHelper webHelper,
          IWorkContext workContext,
          IUrlHelperFactory urlHelperFactory,
          ShoppingCartSettings shoppingCartSettings,
          MyFatoorahPaymentSettings myFatoorahPaymentSettings,
          MyFatoorahHttpClient myFatoorahHttpClient)
        {
            this._actionContextAccessor = actionContextAccessor;
            this._genericAttributeService = genericAttributeService;
            this._orderProcessingService = orderProcessingService;
            this._orderService = orderService;
            this._paymentPluginManager = paymentPluginManager;
            this._permissionService = permissionService;
            this._localizationService = localizationService;
            this._logger = logger;
            this._notificationService = notificationService;
            this._settingService = settingService;
            this._storeContext = storeContext;
            this._webHelper = webHelper;
            this._workContext = workContext;
            this. _urlHelperFactory = urlHelperFactory;
            this._shoppingCartSettings = shoppingCartSettings;
            this._myFatoorahPaymentSettings = myFatoorahPaymentSettings;
            this._myFatoorahHttpClient = myFatoorahHttpClient;
        }

        protected virtual async Task ProcessRecurringPayment(
          string invoiceId,
          PaymentStatus newPaymentStatus,
          string transactionId,
          string ipnInfo)
        {
            Guid guid;
            try
            {
                guid = new Guid(invoiceId);
            }
            catch
            {
                guid = Guid.Empty;
            }
            Order order = await this._orderService.GetOrderByGuidAsync(guid);
            if (order == null)
            {
                await this._logger.ErrorAsync("MyFatoorah. Order is not found", (Exception)new NopException(ipnInfo), (Customer)null);
                order = (Order)null;
            }
            else
            {
                foreach (RecurringPayment rp in (IEnumerable<RecurringPayment>)await this._orderService.SearchRecurringPaymentsAsync(0, 0, ((BaseEntity)order).Id, new OrderStatus?(), 0, int.MaxValue, false))
                {
                    PaymentStatus paymentStatus = newPaymentStatus;
                    if ((int)paymentStatus != 20 && (int)paymentStatus != 30)
                    {
                        if ((int)paymentStatus == 50)
                        {
                            ProcessPaymentResult processPaymentResult = new ProcessPaymentResult()
                            {
                                Errors = (IList<string>)new string[1]
                              {
                  "MyFatoorah. Recurring payment is " + "Voided".ToLower() + " ."
                              },
                                RecurringPaymentFailed = true
                            };
                            IEnumerable<string> strings = await this._orderProcessingService.ProcessNextRecurringPaymentAsync(rp, processPaymentResult);
                        }
                    }
                    else if (!(await this._orderService.GetRecurringPaymentHistoryAsync(rp)).Any<RecurringPaymentHistory>())
                    {
                        await this._orderService.InsertRecurringPaymentHistoryAsync(new RecurringPaymentHistory()
                        {
                            RecurringPaymentId = ((BaseEntity)rp).Id,
                            OrderId = ((BaseEntity)order).Id,
                            CreatedOnUtc = DateTime.UtcNow
                        });
                    }
                    else
                    {
                        ProcessPaymentResult processPaymentResult = new ProcessPaymentResult()
                        {
                            NewPaymentStatus = newPaymentStatus
                        };
                        if ((int)newPaymentStatus == 20)
                            processPaymentResult.AuthorizationTransactionId = transactionId;
                        else
                            processPaymentResult.CaptureTransactionId = transactionId;
                        IEnumerable<string> strings = await this._orderProcessingService.ProcessNextRecurringPaymentAsync(rp, processPaymentResult);
                    }
                }
                await this._logger.InformationAsync("MyFatoorah. Recurring info", (Exception)new NopException(ipnInfo), (Customer)null);
                order = (Order)null;
            }
        }
        public string FormatString(Decimal mcGross, Order order)
        {
            var interpolatedStringHandler = new DefaultInterpolatedStringHandler(70, 3);
            interpolatedStringHandler.AppendLiteral("MyFatoorah. Returned order total ");
            interpolatedStringHandler.AppendFormatted<Decimal>(mcGross);
            interpolatedStringHandler.AppendLiteral(" doesn't equal order total ");
            interpolatedStringHandler.AppendFormatted<Decimal>(order.OrderTotal);
            interpolatedStringHandler.AppendLiteral(". Order# ");
            interpolatedStringHandler.AppendFormatted<int>(((BaseEntity)order).Id);
            interpolatedStringHandler.AppendLiteral(".");
            string errorStr = interpolatedStringHandler.ToStringAndClear();
            return errorStr;
        }
        protected virtual async Task ProcessPayment(
          string orderNumber,
          string ipnInfo,
          PaymentStatus newPaymentStatus,
          Decimal mcGross,
          string transactionId)
        {
            Guid guid;
            try
            {
                guid = new Guid(orderNumber);
            }
            catch
            {
                guid = Guid.Empty;
            }
            Order order = await this._orderService.GetOrderByGuidAsync(guid);
            if (order == null)
            {
                await this._logger.ErrorAsync("MyFatoorah. Order is not found", (Exception)new NopException(ipnInfo), (Customer)null);
                order = (Order)null;
            }
            else
            {
                await this._orderService.InsertOrderNoteAsync(new OrderNote()
                {
                    OrderId = ((BaseEntity)order).Id,
                    Note = ipnInfo,
                    DisplayToCustomer = false,
                    CreatedOnUtc = DateTime.UtcNow
                });
                Decimal num;
                if ((int)newPaymentStatus == 20 || (int)newPaymentStatus == 30)
                {
                    num = Math.Round(mcGross, 2);
                    if (!num.Equals(Math.Round(order.OrderTotal, 2)))
                    {
                        // DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(70, 3);
                        // interpolatedStringHandler.AppendLiteral("MyFatoorah. Returned order total ");
                        // interpolatedStringHandler.AppendFormatted<Decimal>(mcGross);
                        // interpolatedStringHandler.AppendLiteral(" doesn't equal order total ");
                        // interpolatedStringHandler.AppendFormatted<Decimal>(order.OrderTotal);
                        // interpolatedStringHandler.AppendLiteral(". Order# ");
                        // interpolatedStringHandler.AppendFormatted<int>(((BaseEntity)order).Id);
                        // interpolatedStringHandler.AppendLiteral(".");
                        // string errorStr = interpolatedStringHandler.ToStringAndClear();
                        string errorStr = FormatString(mcGross, order);
                        await this._logger.ErrorAsync(errorStr, (Exception)null, (Customer)null);
                        await this._orderService.InsertOrderNoteAsync(new OrderNote()
                        {
                            OrderId = ((BaseEntity)order).Id,
                            Note = errorStr,
                            DisplayToCustomer = false,
                            CreatedOnUtc = DateTime.UtcNow
                        });
                        order = (Order)null;
                        return;
                    }
                }
                PaymentStatus paymentStatus = newPaymentStatus;
                if ((int)paymentStatus <= 30)
                {
                    if ((int)paymentStatus != 20)
                    {
                        if ((int)paymentStatus != 30)
                            order = (Order)null;
                        else if (!this._orderProcessingService.CanMarkOrderAsPaid(order))
                        {
                            order = (Order)null;
                        }
                        else
                        {
                            order.AuthorizationTransactionId = transactionId;
                            await this._orderService.UpdateOrderAsync(order);
                            await this._orderProcessingService.MarkOrderAsPaidAsync(order);
                            order = (Order)null;
                        }
                    }
                    else if (!this._orderProcessingService.CanMarkOrderAsAuthorized(order))
                    {
                        order = (Order)null;
                    }
                    else
                    {
                        await this._orderProcessingService.MarkAsAuthorizedAsync(order);
                        order = (Order)null;
                    }
                }
                else if ((int)paymentStatus != 40)
                {
                    if ((int)paymentStatus != 50)
                        order = (Order)null;
                    else if (!this._orderProcessingService.CanVoidOffline(order))
                    {
                        order = (Order)null;
                    }
                    else
                    {
                        await this._orderProcessingService.VoidOfflineAsync(order);
                        order = (Order)null;
                    }
                }
                else
                {
                    Decimal d = Math.Abs(mcGross);
                    if (d > 0M)
                    {
                        num = Math.Round(d, 2);
                        if (num.Equals(Math.Round(order.OrderTotal, 2)))
                        {
                            if (!this._orderProcessingService.CanRefundOffline(order))
                            {
                                order = (Order)null;
                                return;
                            }
                            await this._orderProcessingService.RefundOfflineAsync(order);
                            order = (Order)null;
                            return;
                        }
                    }
                    if (!this._orderProcessingService.CanPartiallyRefundOffline(order, d))
                    {
                        order = (Order)null;
                    }
                    else
                    {
                        await this._orderProcessingService.PartiallyRefundOfflineAsync(order, d);
                        order = (Order)null;
                    }
                }
            }
        }

        [AuthorizeAdmin(false)]
        [Area("Admin")]
        public async Task<IActionResult> Configure()
        {
            PaymentMyFatoorahController fatoorahController = this;
            if (!await fatoorahController._permissionService.AuthorizeAsync(StandardPermissionProvider.ManagePaymentMethods))
                // return ((BaseController)fatoorahController).AccessDeniedView();
                return Unauthorized();
            int storeScope = await fatoorahController._storeContext.GetActiveStoreScopeConfigurationAsync();
            MyFatoorahPaymentSettings myFatoorahPaymentSettings = await fatoorahController._settingService.LoadSettingAsync<MyFatoorahPaymentSettings>(storeScope);
            ConfigurationModel model = new ConfigurationModel()
            {
                UseSandbox = myFatoorahPaymentSettings.UseSandbox,
                APIToken = myFatoorahPaymentSettings.APIToken,
                AdditionalFee = myFatoorahPaymentSettings.AdditionalFee,
                AdditionalFeePercentage = myFatoorahPaymentSettings.AdditionalFeePercentage,
                ReturnFromMyFatoorahWithoutPaymentRedirectsToOrderDetailsPage = myFatoorahPaymentSettings.ReturnFromMyFatoorahWithoutPaymentRedirectsToOrderDetailsPage,
                DisplayCurrencyIsoAlpha = myFatoorahPaymentSettings.DisplayCurrencyIsoAlpha,
                PaymentAcknowledgement = myFatoorahPaymentSettings.SendInvoiceOption,
                Country = myFatoorahPaymentSettings.Country,
                ActiveStoreScopeConfiguration = storeScope
            };
            if (storeScope <= 0)
                return (IActionResult)((Controller)fatoorahController).View("~/Plugins/Payments.MyFatoorah/Views/Configure.cshtml", (object)model);
            ConfigurationModel configurationModel = model;
            ISettingService settingService1 = fatoorahController._settingService;
            MyFatoorahPaymentSettings fatoorahPaymentSettings1 = myFatoorahPaymentSettings;
            Expression<Func<MyFatoorahPaymentSettings, bool>> expression1 = (Expression<Func<MyFatoorahPaymentSettings, bool>>)(x => x.UseSandbox);
            int num1 = storeScope;
            configurationModel.UseSandbox_OverrideForStore = await settingService1.SettingExistsAsync<MyFatoorahPaymentSettings, bool>(fatoorahPaymentSettings1, expression1, num1);
            configurationModel = (ConfigurationModel)null;
            configurationModel = model;
            ISettingService settingService2 = fatoorahController._settingService;
            MyFatoorahPaymentSettings fatoorahPaymentSettings2 = myFatoorahPaymentSettings;
            Expression<Func<MyFatoorahPaymentSettings, string>> expression2 = (Expression<Func<MyFatoorahPaymentSettings, string>>)(x => x.APIToken);
            int num2 = storeScope;
            configurationModel.APIToken_OverrideForStore = await settingService2.SettingExistsAsync<MyFatoorahPaymentSettings, string>(fatoorahPaymentSettings2, expression2, num2);
            configurationModel = (ConfigurationModel)null;
            configurationModel = model;
            ISettingService settingService3 = fatoorahController._settingService;
            MyFatoorahPaymentSettings fatoorahPaymentSettings3 = myFatoorahPaymentSettings;
            Expression<Func<MyFatoorahPaymentSettings, string>> expression3 = (Expression<Func<MyFatoorahPaymentSettings, string>>)(x => x.APIToken);
            int num3 = storeScope;
            configurationModel.APIToken_OverrideForStore = await settingService3.SettingExistsAsync<MyFatoorahPaymentSettings, string>(fatoorahPaymentSettings3, expression3, num3);
            configurationModel = (ConfigurationModel)null;
            configurationModel = model;
            ISettingService settingService4 = fatoorahController._settingService;
            MyFatoorahPaymentSettings fatoorahPaymentSettings4 = myFatoorahPaymentSettings;
            Expression<Func<MyFatoorahPaymentSettings, Decimal>> expression4 = (Expression<Func<MyFatoorahPaymentSettings, Decimal>>)(x => x.AdditionalFee);
            int num4 = storeScope;
            configurationModel.AdditionalFee_OverrideForStore = await settingService4.SettingExistsAsync<MyFatoorahPaymentSettings, Decimal>(fatoorahPaymentSettings4, expression4, num4);
            configurationModel = (ConfigurationModel)null;
            configurationModel = model;
            ISettingService settingService5 = fatoorahController._settingService;
            MyFatoorahPaymentSettings fatoorahPaymentSettings5 = myFatoorahPaymentSettings;
            Expression<Func<MyFatoorahPaymentSettings, bool>> expression5 = (Expression<Func<MyFatoorahPaymentSettings, bool>>)(x => x.AdditionalFeePercentage);
            int num5 = storeScope;
            configurationModel.AdditionalFeePercentage_OverrideForStore = await settingService5.SettingExistsAsync<MyFatoorahPaymentSettings, bool>(fatoorahPaymentSettings5, expression5, num5);
            configurationModel = (ConfigurationModel)null;
            configurationModel = model;
            ISettingService settingService6 = fatoorahController._settingService;
            MyFatoorahPaymentSettings fatoorahPaymentSettings6 = myFatoorahPaymentSettings;
            Expression<Func<MyFatoorahPaymentSettings, string>> expression6 = (Expression<Func<MyFatoorahPaymentSettings, string>>)(x => x.DisplayCurrencyIsoAlpha);
            int num6 = storeScope;
            configurationModel.DisplayCurrencyIsoAlpha_OverrideForStore = await settingService6.SettingExistsAsync<MyFatoorahPaymentSettings, string>(fatoorahPaymentSettings6, expression6, num6);
            configurationModel = (ConfigurationModel)null;
            configurationModel = model;
            ISettingService settingService7 = fatoorahController._settingService;
            MyFatoorahPaymentSettings fatoorahPaymentSettings7 = myFatoorahPaymentSettings;
            Expression<Func<MyFatoorahPaymentSettings, int?>> expression7 = (Expression<Func<MyFatoorahPaymentSettings, int?>>)(x => x.SendInvoiceOption);
            int num7 = storeScope;
            configurationModel.PaymentAcknowledgement_OverrideForStore = await settingService7.SettingExistsAsync<MyFatoorahPaymentSettings, int?>(fatoorahPaymentSettings7, expression7, num7);
            configurationModel = (ConfigurationModel)null;
            configurationModel = model;
            ISettingService settingService8 = fatoorahController._settingService;
            MyFatoorahPaymentSettings fatoorahPaymentSettings8 = myFatoorahPaymentSettings;
            Expression<Func<MyFatoorahPaymentSettings, bool>> expression8 = (Expression<Func<MyFatoorahPaymentSettings, bool>>)(x => x.ReturnFromMyFatoorahWithoutPaymentRedirectsToOrderDetailsPage);
            int num8 = storeScope;
            configurationModel.ReturnFromMyFatoorahWithoutPaymentRedirectsToOrderDetailsPage_OverrideForStore = await settingService8.SettingExistsAsync<MyFatoorahPaymentSettings, bool>(fatoorahPaymentSettings8, expression8, num8);
            configurationModel = (ConfigurationModel)null;
            configurationModel = model;
            ISettingService settingService9 = fatoorahController._settingService;
            MyFatoorahPaymentSettings fatoorahPaymentSettings9 = myFatoorahPaymentSettings;
            Expression<Func<MyFatoorahPaymentSettings, string>> expression9 = (Expression<Func<MyFatoorahPaymentSettings, string>>)(x => x.Country);
            int num9 = storeScope;
            configurationModel.Country_OverrideForStore = await settingService9.SettingExistsAsync<MyFatoorahPaymentSettings, string>(fatoorahPaymentSettings9, expression9, num9);
            configurationModel = (ConfigurationModel)null;
            return (IActionResult)((Controller)fatoorahController).View("~/Plugins/Payments.MyFatoorah/Views/Configure.cshtml", (object)model);
        }

        [HttpPost]
        [AuthorizeAdmin(false)]
        [Area("Admin")]
        public async Task<IActionResult> Configure(ConfigurationModel model)
        {
            PaymentMyFatoorahController fatoorahController = this;
            if (!await fatoorahController._permissionService.AuthorizeAsync(StandardPermissionProvider.ManagePaymentMethods))
                //return ((BaseController) fatoorahController).AccessDeniedView();
                return Unauthorized();
            if (!((ControllerBase)fatoorahController).ModelState.IsValid)
                return await fatoorahController.Configure();
            int storeScope = await fatoorahController._storeContext.GetActiveStoreScopeConfigurationAsync();
            MyFatoorahPaymentSettings myFatoorahPaymentSettings = await fatoorahController._settingService.LoadSettingAsync<MyFatoorahPaymentSettings>(storeScope);
            if (model.APIToken.StartsWith("bearer", true, (CultureInfo)null))
                model.APIToken = model.APIToken.Replace("bearer", "");
            myFatoorahPaymentSettings.UseSandbox = model.UseSandbox;
            myFatoorahPaymentSettings.APIToken = model.APIToken.Trim();
            myFatoorahPaymentSettings.AdditionalFee = model.AdditionalFee;
            myFatoorahPaymentSettings.AdditionalFeePercentage = model.AdditionalFeePercentage;
            myFatoorahPaymentSettings.DisplayCurrencyIsoAlpha = model.DisplayCurrencyIsoAlpha;
            myFatoorahPaymentSettings.SendInvoiceOption = new int?((int)Convert.ToInt16((object)model.PaymentAcknowledgement));
            myFatoorahPaymentSettings.Country = model.Country;
            myFatoorahPaymentSettings.ReturnFromMyFatoorahWithoutPaymentRedirectsToOrderDetailsPage = model.ReturnFromMyFatoorahWithoutPaymentRedirectsToOrderDetailsPage;
            await fatoorahController._settingService.SaveSettingAsync<MyFatoorahPaymentSettings>(myFatoorahPaymentSettings, 0);
            await fatoorahController._settingService.SaveSettingOverridablePerStoreAsync<MyFatoorahPaymentSettings, bool>(myFatoorahPaymentSettings, (Expression<Func<MyFatoorahPaymentSettings, bool>>)(x => x.UseSandbox), (model.UseSandbox_OverrideForStore ? 1 : 0) != 0, storeScope, false);
            await fatoorahController._settingService.SaveSettingOverridablePerStoreAsync<MyFatoorahPaymentSettings, string>(myFatoorahPaymentSettings, (Expression<Func<MyFatoorahPaymentSettings, string>>)(x => x.APIToken), (model.APIToken_OverrideForStore ? 1 : 0) != 0, storeScope, false);
            await fatoorahController._settingService.SaveSettingOverridablePerStoreAsync<MyFatoorahPaymentSettings, Decimal>(myFatoorahPaymentSettings, (Expression<Func<MyFatoorahPaymentSettings, Decimal>>)(x => x.AdditionalFee), (model.AdditionalFee_OverrideForStore ? 1 : 0) != 0, storeScope, false);
            await fatoorahController._settingService.SaveSettingOverridablePerStoreAsync<MyFatoorahPaymentSettings, bool>(myFatoorahPaymentSettings, (Expression<Func<MyFatoorahPaymentSettings, bool>>)(x => x.AdditionalFeePercentage), (model.AdditionalFeePercentage_OverrideForStore ? 1 : 0) != 0, storeScope, false);
            await fatoorahController._settingService.SaveSettingOverridablePerStoreAsync<MyFatoorahPaymentSettings, string>(myFatoorahPaymentSettings, (Expression<Func<MyFatoorahPaymentSettings, string>>)(x => x.DisplayCurrencyIsoAlpha), (model.DisplayCurrencyIsoAlpha_OverrideForStore ? 1 : 0) != 0, storeScope, false);
            await fatoorahController._settingService.SaveSettingOverridablePerStoreAsync<MyFatoorahPaymentSettings, bool>(myFatoorahPaymentSettings, (Expression<Func<MyFatoorahPaymentSettings, bool>>)(x => x.ReturnFromMyFatoorahWithoutPaymentRedirectsToOrderDetailsPage), (model.ReturnFromMyFatoorahWithoutPaymentRedirectsToOrderDetailsPage_OverrideForStore ? 1 : 0) != 0, storeScope, false);
            await fatoorahController._settingService.SaveSettingOverridablePerStoreAsync<MyFatoorahPaymentSettings, string>(myFatoorahPaymentSettings, (Expression<Func<MyFatoorahPaymentSettings, string>>)(x => x.Country), (model.Country_OverrideForStore ? 1 : 0) != 0, storeScope, false);
            await fatoorahController._settingService.ClearCacheAsync();
            INotificationService inotificationService = fatoorahController._notificationService;
            inotificationService.SuccessNotification(await fatoorahController._localizationService.GetResourceAsync("Admin.Plugins.Saved"), true);
            inotificationService = (INotificationService)null;
            return await fatoorahController.Configure();
        }

        [AuthorizeAdmin(false)]
        [Area("Admin")]
        public async Task<IActionResult> RoundingWarning(bool passProductNamesAndTotals)
        {
            PaymentMyFatoorahController fatoorahController = this;
            if (!await fatoorahController._permissionService.AuthorizeAsync(StandardPermissionProvider.ManagePaymentMethods))
                //return ((BaseController)fatoorahController).AccessDeniedView();
                return Unauthorized();
            if (!passProductNamesAndTotals || fatoorahController._shoppingCartSettings.RoundPricesDuringCalculation)
                return (IActionResult)((Controller)fatoorahController).Json((object)new
                {
                    Result = string.Empty
                });
            string resourceAsync = await fatoorahController._localizationService.GetResourceAsync("Plugins.Payments.MyFatoorah.RoundingWarning");
            return (IActionResult)((Controller)fatoorahController).Json((object)new
            {
                Result = resourceAsync
            });
        }

        public async Task<IActionResult> PDTHandler(string paymentId, string Id)
        {
            PaymentMyFatoorahController fatoorahController = this;
            int num1;
            try
            {
                (bool, Order) valueTuple = await ValidateOrderResponseAsync();
                int num2 = valueTuple.Item1 ? 1 : 0;
                Order order = valueTuple.Item2;
                if (num2 == 0)
                    return (IActionResult)((ControllerBase)fatoorahController).RedirectToAction("Homepage");
                if (!fatoorahController._orderProcessingService.CanMarkOrderAsPaid(order))
                    return (IActionResult)((ControllerBase)fatoorahController).RedirectToRoute("CheckoutCompleted", (object)new
                    {
                        orderId = ((BaseEntity)order).Id
                    });
                await fatoorahController._orderService.UpdateOrderAsync(order);
                await fatoorahController._orderProcessingService.MarkOrderAsPaidAsync(order);
                return (IActionResult)((ControllerBase)fatoorahController).RedirectToRoute("CheckoutCompleted", (object)new
                {
                    orderId = ((BaseEntity)order).Id
                });
            }
            catch (Exception ex)
            {
                num1 = 1;
                await fatoorahController._logger.ErrorAsync("Error while getting response from MyFatoorah", ex, (Customer)null);
                return (IActionResult)((ControllerBase)fatoorahController).RedirectToAction("Homepage");
            }
            if (num1 != 1)
            {
                IActionResult actionResult;
                return actionResult;
            }

        }

        public async Task<IActionResult> CancelOrder()
        {
            PaymentMyFatoorahController fatoorahController = this;
            (bool, Order) valueTuple = await fatoorahController.ValidateOrderResponseAsync();
            int num = valueTuple.Item1 ? 1 : 0;
            Order order = valueTuple.Item2;
            if (num == 0)
                return (IActionResult)((ControllerBase)fatoorahController).RedirectToAction("Homepage");
            await fatoorahController._orderProcessingService.CancelOrderAsync(order, true);
            return (IActionResult)((ControllerBase)fatoorahController).RedirectToRoute("OrderDetails", (object)new
            {
                orderId = ((BaseEntity)order).Id
            });
        }

        private async Task<(bool isValid, Order order)> ValidateOrderResponseAsync()
        {
            ValueTuple<bool, Order> valueTuple;
            bool flag;
            Order transactionId;
            string str;
            string str1;
            string str2 = this._webHelper.QueryString<string>("id");
            var urlHelper = _urlHelperFactory.GetUrlHelper(_actionContextAccessor.ActionContext);
            IPaymentMethod paymentMethod = await this._paymentPluginManager.LoadPluginBySystemNameAsync("Payments.MyFatoorah.KonnectAll", null, 0);
            IPaymentMethod paymentMethod1 = paymentMethod;
            MyFatoorahPaymentProcessor myFatoorahPaymentProcessor = paymentMethod1 as MyFatoorahPaymentProcessor;
            flag = (myFatoorahPaymentProcessor == null ? true : !this._paymentPluginManager.IsPluginActive(myFatoorahPaymentProcessor));
            if (flag)
            {
                paymentMethod1 = null;
                paymentMethod = null;
                throw new NopException("MyFatoorah module cannot be loaded");
            }
            GetPaymentStatusResponse result = await _myFatoorahHttpClient.GetPaymentStatusAsync(str2);
            if (result != null)
            {
                IOrderService orderService = this._orderService;
                long invoiceId = result.Data.InvoiceId;
                Order orderByCustomOrderNumberAsync = await orderService.GetOrderByCustomOrderNumberAsync(invoiceId.ToString());
                transactionId = orderByCustomOrderNumberAsync;
                orderByCustomOrderNumberAsync = null;
                if (transactionId != null)
                {
                    if (result.Data.InvoiceTransactions.Any<InvoiceTransaction>())
                    {
                        IList<InvoiceTransaction> invoiceTransactions = result.Data.InvoiceTransactions;
                        InvoiceTransaction invoiceTransaction = invoiceTransactions.SingleOrDefault<InvoiceTransaction>((InvoiceTransaction x) => x.TransactionStatus == "Succss");
                        if (invoiceTransaction == null)
                        {
                            IList<InvoiceTransaction> invoiceTransactions1 = result.Data.InvoiceTransactions;
                            InvoiceTransaction invoiceTransaction1 = invoiceTransactions1.LastOrDefault<InvoiceTransaction>((InvoiceTransaction x) => x.TransactionStatus != "Succss");
                            result.Message = invoiceTransaction1.Error;
                            invoiceTransaction = null;
                            invoiceTransaction1 = null;
                        }
                        else
                        {
                            transactionId.AuthorizationTransactionId = invoiceTransaction.TransactionId;
                            valueTuple = new ValueTuple<bool, Order>(true, transactionId);
                            str2 = null;
                            myFatoorahPaymentProcessor = null;
                            result = null;
                            transactionId = null;
                            str = null;
                            str1 = null;
                            return valueTuple;
                        }
                    }
                    str = urlHelper.RouteUrl("ReOrder", new { orderId = transactionId.Id });
                    str1 = string.Concat(result.MessageSummary, ", Your order has been cancelled due to failure payment. Please try to <a href='", str, "'>Re-Order</a>");
                    IOrderService orderService1 = this._orderService;
                    OrderNote orderNote = new OrderNote()
                    {
                        OrderId = transactionId.Id,
                        Note = string.Concat("MyFatoorah payment failed. ", result.MessageSummary),
                        DisplayToCustomer = false,
                        CreatedOnUtc = DateTime.UtcNow
                    };
                    await orderService1.InsertOrderNoteAsync(orderNote);
                    this._notificationService.ErrorNotification(str1, true);
                    valueTuple = new ValueTuple<bool, Order>(true, transactionId);
                }
                else
                {
                    valueTuple = new ValueTuple<bool, Order>(false, (Order)null);
                }
            }
            else
            {
                valueTuple = new ValueTuple<bool, Order>(false, (Order)null);
            }
            str2 = null;
            myFatoorahPaymentProcessor = null;
            result = null;
            transactionId = null;
            str = null;
            str1 = null;
            return valueTuple;
        }
    }
}
