using Nop.Services.Orders;
using Nop.Core.Domain.Orders;
using Iyzipay.Model;
using Nop.Core;
using Iyzipay.Request;
using Iyzipay;

namespace Konnectall.Plugin.Payment.Services
{
    public class IyzicoPaymentService : IIyzicoPaymentService
    {
        #region Fields
        private readonly IWorkContext _workContext;
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IOrderTotalCalculationService _orderTotalCalculationService;
        private readonly IStoreContext _storeContext;
        private readonly IyzicoSettings _iyzicoSettings;
        #endregion
        #region Ctor
        public IyzicoPaymentService
        (
            IWorkContext workContext,
            IStoreContext storeContext,
            IyzicoSettings iyzicoSettings,
            IShoppingCartService shoppingCartService,
            IOrderTotalCalculationService orderTotalCalculationService
        )
        {
            _workContext = workContext;
            _shoppingCartService = shoppingCartService;
            _orderTotalCalculationService = orderTotalCalculationService;
            _storeContext = storeContext;
            _iyzicoSettings = iyzicoSettings;
        }


        #endregion
        #region Methods
        public async Task<Locale> GetcaleFromCurrentLanguageAsync()
        {
            var currentLanguage = await _workContext.GetWorkingLanguageAsync();
            if (currentLanguage.LanguageCulture.Contains("tr"))
                return Locale.TR;
            return Locale.EN;
        }
        public async Task<decimal?> GetShoppingCartTotalAsync()
        {
            // get current store
            var store = await _storeContext.GetCurrentStoreAsync();
            var storeId = store.Id;
            // get current customer
            var currentCustomer = await _workContext.GetCurrentCustomerAsync();
            // get current customer's shopping cart items
            var cart = await _shoppingCartService.GetShoppingCartAsync(currentCustomer, ShoppingCartType.ShoppingCart, storeId);
            var (total, _, _, _, _, _) = await _orderTotalCalculationService.GetShoppingCartTotalAsync(cart, null, false);

            return total ?? 0m;
        }
        public async Task<InstallmentInfo> GetInstallmentsAsync(Guid conversationId, string binNumber)
        {
            var locale = GetcaleFromCurrentLanguageAsync();
            var shopingCartTotal = await GetShoppingCartTotalAsync();
            var request = new RetrieveInstallmentInfoRequest
            {
                Locale = locale.ToString(),
                ConversationId = conversationId.ToString(),
                BinNumber = binNumber,
                Price = (shopingCartTotal ?? 0m).ToString()
            };
            var options = new Options
            {
                ApiKey = _iyzicoSettings.ApiKey,
                SecretKey = _iyzicoSettings.SecretKey,
                BaseUrl = _iyzicoSettings.Uri
            };
            var installmentInfo = InstallmentInfo.Retrieve(request, options);
            return installmentInfo;
        }
        #endregion
    }
}