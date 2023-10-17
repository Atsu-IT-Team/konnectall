using System;
using Konnectall.Plugin.Payment.Models;

namespace Konnectall.Plugin.Payment.Factories
{
    public interface IIyzicoPaymentModelFactory
    {
        Task<InstallmentModel> PrepareInstallmentModelAsync(string binNumber);
    }
}
