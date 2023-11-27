using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Directory;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Shipping;
using Nop.Core.Domain.Tax;
using Nop.Plugin.Widgets.KonnectAll.Features.Services;
using Nop.Services.Affiliates;
using Nop.Services.Catalog;
using Nop.Services.Common;
using Nop.Services.Customers;
using Nop.Services.Directory;
using Nop.Services.Discounts;
using Nop.Services.Helpers;
using Nop.Services.Localization;
using Nop.Services.Media;
using Nop.Services.Orders;
using Nop.Services.Payments;
using Nop.Services.Security;
using Nop.Services.Seo;
using Nop.Services.Shipping;
using Nop.Services.Stores;
using Nop.Services.Tax;
using Nop.Services.Vendors;
using Nop.Web.Areas.Admin.Factories;
using Nop.Web.Areas.Admin.Models.Orders;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.KonnectAll.Features.Factories
{
    public class CustomOrderModelFactory : OrderModelFactory
    {
        #region Fields
        private readonly IAffiliateService _affiliateService;
        private readonly ICustomerService _customerService;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly ILocalizationService _localizationService;
        private readonly IPaymentService _paymentService;
        private readonly IPriceFormatter _priceFormatter;
        private readonly IStoreService _storeService;
        private readonly IWorkContext _workContext;
        private readonly TaxSettings _taxSettings;
        private readonly IKonnectAllService _konnectAllService;
        #endregion

        #region Ctor
        public CustomOrderModelFactory(
            AddressSettings addressSettings,
            CatalogSettings catalogSettings,
            CurrencySettings currencySettings,
            IActionContextAccessor actionContextAccessor,
            IAddressModelFactory addressModelFactory,
            IAddressService addressService,
            IAffiliateService affiliateService,
            IBaseAdminModelFactory baseAdminModelFactory,
            ICountryService countryService,
            ICurrencyService currencyService,
            ICustomerService customerService,
            IDateTimeHelper dateTimeHelper,
            IDiscountService discountService,
            IDownloadService downloadService,
            IEncryptionService encryptionService,
            IGiftCardService giftCardService,
            ILocalizationService localizationService,
            IMeasureService measureService,
            IOrderProcessingService orderProcessingService,
            IOrderReportService orderReportService,
            IOrderService orderService,
            IPaymentPluginManager paymentPluginManager,
            IPaymentService paymentService,
            IPictureService pictureService,
            IPriceCalculationService priceCalculationService,
            IPriceFormatter priceFormatter,
            IProductAttributeService productAttributeService,
            IProductService productService,
            IReturnRequestService returnRequestService,
            IRewardPointService rewardPointService,
            IShipmentService shipmentService,
            IShippingService shippingService,
            IStateProvinceService stateProvinceService,
            IStoreService storeService,
            ITaxService taxService,
            IUrlHelperFactory urlHelperFactory,
            IVendorService vendorService,
            IWorkContext workContext,
            MeasureSettings measureSettings,
            OrderSettings orderSettings,
            ShippingSettings shippingSettings,
            IUrlRecordService urlRecordService,
            TaxSettings taxSettings,
            IKonnectAllService konnectAllService) : base(
                addressSettings,
                catalogSettings,
                currencySettings,
                actionContextAccessor,
                addressModelFactory,
                addressService,
                affiliateService,
                baseAdminModelFactory,
                countryService,
                currencyService,
                customerService,
                dateTimeHelper,
                discountService,
                downloadService,
                encryptionService,
                giftCardService,
                localizationService,
                measureService,
                orderProcessingService,
                orderReportService,
                orderService,
                paymentPluginManager,
                paymentService,
                pictureService,
                priceCalculationService,
                priceFormatter,
                productAttributeService,
                productService,
                returnRequestService,
                rewardPointService,
                shipmentService,
                shippingService,
                stateProvinceService,
                storeService,
                taxService,
                urlHelperFactory,
                vendorService,
                workContext,
                measureSettings,
                orderSettings,
                shippingSettings,
                urlRecordService,
                taxSettings)
        {
            _affiliateService = affiliateService;
            _customerService = customerService;
            _dateTimeHelper = dateTimeHelper;
            _localizationService = localizationService;
            _paymentService = paymentService;
            _priceFormatter = priceFormatter;
            _workContext = workContext;
            _storeService = storeService;
            _taxSettings = taxSettings;
            _konnectAllService = konnectAllService;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Prepare order model
        /// </summary>
        /// <param name="model">Order model</param>
        /// <param name="order">Order</param>
        /// <param name="excludeProperties">Whether to exclude populating of some properties of model</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the order model
        /// </returns>
        public override async Task<OrderModel> PrepareOrderModelAsync(OrderModel model, Order order, bool excludeProperties = false)
        {
            if (order != null)
            {
                //fill in model values from the entity
                model ??= new OrderModel
                {
                    Id = order.Id,
                    OrderStatusId = order.OrderStatusId,
                    VatNumber = order.VatNumber,
                    CheckoutAttributeInfo = order.CheckoutAttributeDescription
                };

                var customer = await _customerService.GetCustomerByIdAsync(order.CustomerId);

                model.OrderGuid = order.OrderGuid;
                model.CustomOrderNumber = order.CustomOrderNumber;
                model.CustomerIp = order.CustomerIp;
                model.CustomerId = customer.Id;
                model.OrderStatus = await _localizationService.GetLocalizedEnumAsync(order.OrderStatus);
                model.StoreName = (await _storeService.GetStoreByIdAsync(order.StoreId))?.Name ?? "Deleted";
                model.CustomerInfo = await _customerService.IsRegisteredAsync(customer) ? customer.Email : await _localizationService.GetResourceAsync("Admin.Customers.Guest");
                model.CreatedOn = await _dateTimeHelper.ConvertToUserTimeAsync(order.CreatedOnUtc, DateTimeKind.Utc);
                model.CustomValues = _paymentService.DeserializeCustomValues(order);

                //get commission
                int vendorId = 0;
                var vendor = await _workContext.GetCurrentVendorAsync();
                
                if (vendor != null)
                    vendorId = vendor.Id;

                var commission = await _konnectAllService.GetCommissionByOrderIdAndVendorAsync(model.Id, vendorId);
                if (commission > 0)
                {
                    var ca = await _priceFormatter.FormatPriceAsync(commission, true, false);
                    model.CustomValues.Add("Commission", ca);
                }

                var affiliate = await _affiliateService.GetAffiliateByIdAsync(order.AffiliateId);
                if (affiliate != null)
                {
                    model.AffiliateId = affiliate.Id;
                    model.AffiliateName = await _affiliateService.GetAffiliateFullNameAsync(affiliate);
                }

                //prepare order totals
                await PrepareOrderModelTotalsAsync(model, order);

                //prepare order items
                await PrepareOrderItemModelsAsync(model.Items, order);
                model.HasDownloadableProducts = model.Items.Any(item => item.IsDownload);

                //prepare payment info
                await PrepareOrderModelPaymentInfoAsync(model, order);

                //prepare shipping info
                await PrepareOrderModelShippingInfoAsync(model, order);

                //prepare nested search model
                PrepareOrderShipmentSearchModel(model.OrderShipmentSearchModel, order);
                PrepareOrderNoteSearchModel(model.OrderNoteSearchModel, order);
            }

            model.IsLoggedInAsVendor = await _workContext.GetCurrentVendorAsync() != null;
            model.AllowCustomersToSelectTaxDisplayType = _taxSettings.AllowCustomersToSelectTaxDisplayType;
            model.TaxDisplayType = _taxSettings.TaxDisplayType;

            return model;
        }
        #endregion
    }
}
