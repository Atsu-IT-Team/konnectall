using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;
using System.Collections.Generic;

namespace Nop.Plugin.Widgets.KonnectAll.Features.Areas.Admin.Models.Commission
{
    public record CommissionSearchModel : BaseSearchModel
    {
        public CommissionSearchModel() 
        {
            AvailableVendors = new List<SelectListItem>();
        }

        [NopResourceDisplayName("Plugin.Widgets.KonnectAll.Features.Commission.Search.Vendor")]
        public int SelectedVendorId { get; set; }
        public IList<SelectListItem> AvailableVendors { get; set; }
    }
}
