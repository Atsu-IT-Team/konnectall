using System;
using Konnectall.Plugin.Payment.Services;
using Konnectall.Plugin.Payment.Models;
namespace Konnectall.Plugin.Payment.Factories
{
    public class IyzicoPaymentModelFactory : IIyzicoPaymentModelFactory
    {
        #region Fields
        private readonly IIyzicoPaymentService _iyzicoPaymentService;
        #endregion
        #region Ctor
        public IyzicoPaymentModelFactory(IIyzicoPaymentService iyzicoPaymentService)
        {
            _iyzicoPaymentService = iyzicoPaymentService;
        }
        #endregion
        #region Methods

        public async Task<InstallmentModel> PrepareInstallmentModelAsync(string binNumber)
        {
            var conversationId = Guid.NewGuid();
            var result = await _iyzicoPaymentService.GetInstallmentsAsync(conversationId, binNumber);
            var model = new InstallmentModel();
            if (result.Status == "success")
            {
                var installmentInfo = result.InstallmentDetails.FirstOrDefault();
                model.Price = installmentInfo.Price;
                model.CardType = installmentInfo.CardType;
                model.CardAssociation = installmentInfo.CardAssociation;
                model.CardFamilyName = installmentInfo.CardFamilyName;
                model.BankCode = installmentInfo.BankCode;
                model.BankName = installmentInfo.BankName;
                model.Force3Ds = installmentInfo.Force3Ds;
                model.ForceCvc = installmentInfo.ForceCvc;
                var installments = installmentInfo.InstallmentPrices;
                model.Prices = installments.Select(p =>
                {
                    return new InstallmentPriceModel
                    {
                        Price = p.Price,
                        TotalPrice = p.TotalPrice,
                        InstallmentNumber = p.InstallmentNumber
                    };
                }).ToList();
            }
            return model;
        }
        #endregion
    }
}
