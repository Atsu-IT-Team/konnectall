using System;
using Nop.Web.Framework.Models;

namespace Konnectall.Plugin.Payment.Models
{
    public record InstallmentPriceModel : BaseNopModel
    {
        public string Price { get; set; }
        public string TotalPrice { get; set; }
        public int? InstallmentNumber { get; set; }
    }
}
