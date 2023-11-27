using Nop.Web.Framework.Models;

namespace Nop.Plugin.Widgets.KonnectAll.Features.Areas.Admin.Models.Commission
{
    public record CommissionModel : BaseNopEntityModel
    {
        public int OrderId { get; set; }
        public int VendorId { get; set; }
        public string VendorName { get; set; }
        public string CommissionRate { get; set; }
        public string CommissionAmount { get; set; }
        public string OrderTotalInclTax { get; set; }
        public string OrderTotalExclTax { get; set; }
    }
}
