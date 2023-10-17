using Nop.Core;
using Nop.Services.Orders;
using Nop.Web.Framework.Controllers;
using Nop.Web.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Domain.Orders;
using Iyzipay.Request;
using Iyzipay.Model;
using Microsoft.AspNetCore.Mvc;
using Iyzipay;
using Konnectall.Plugin.Payment.Services;
using LinqToDB.Tools;
using Konnectall.Plugin.Payment.Factories;
namespace Konnectall.Plugin.Payment.Controllers
{
    public class IyzicoPaymentController : BasePaymentController
    {
        #region Fields
        private readonly IIyzicoPaymentModelFactory _iyzicoPaymentModelFactory;
        #endregion
        #region Ctor
        public IyzicoPaymentController
        (
         IIyzicoPaymentModelFactory iyzicoPaymentModelFactory
        )
        {
            _iyzicoPaymentModelFactory = iyzicoPaymentModelFactory;
        }
        #endregion
        #region Bin Number Methods

        //[HttpPost]
        public async Task<IActionResult> ChecBinNumber(string binNumber)
        {
            var installmentInfo = await _iyzicoPaymentModelFactory.PrepareInstallmentModelAsync(binNumber);
            return Json(new { Success = true, Result = installmentInfo });
        }
        #endregion
    }
}
