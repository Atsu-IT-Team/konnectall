using System;
using Nop.Web.Framework.Models;

namespace Konnectall.Plugin.Payment.Models
{
    public record InstallmentModel : BaseNopModel
    {
        public string Price { get; set; }
        public string CardType { get; set; }
        public string CardAssociation { get; set; }
        public string CardFamilyName { get; set; }
        public int? Force3Ds { get; set; }
        public long? BankCode { get; set; }
        public string BankName { get; set; }
        public int? ForceCvc { get; set; }
        public List<InstallmentPriceModel> Prices { get; set; } = new();
    }
}
