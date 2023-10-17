using Iyzipay.Model;

namespace Konnectall.Plugin.Payment.Services
{
    public interface IIyzicoPaymentService
    {
        Task<Locale> GetcaleFromCurrentLanguageAsync();
        Task<decimal?> GetShoppingCartTotalAsync();
        Task<InstallmentInfo> GetInstallmentsAsync(Guid conversationId, string binNumber);
    }
}