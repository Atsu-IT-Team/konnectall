using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;
using System.Collections.Generic;

namespace Nop.Plugin.Widgets.KonnectAll.Features.Areas.Admin.Models
{
    /// <summary>
    /// Represents a online sales search model
    /// </summary>
    public record OnlineSalesSearchModel : BaseSearchModel
    {
        #region Ctor
        public OnlineSalesSearchModel()
        {
            AvailablePublishedOptions = new List<SelectListItem>();
        }
        #endregion

        #region Properties

        [NopResourceDisplayName("Plugin.Widgets.KonnectAll.Features.OnlineSales.Search.Field.Title")]
        public string Title { get; set; }

        [NopResourceDisplayName("Plugin.Widgets.KonnectAll.Features.OnlineSales.Search.Field.Published")]
        public int PublishedId { get; set; }

        public IList<SelectListItem> AvailablePublishedOptions { get; set; }
        #endregion
    }
}
