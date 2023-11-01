using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Widgets.KonnectAll.Features.Areas.Admin.Models.ApplicationRequest
{
    /// <summary>
    /// Represents a application request search model
    /// </summary>
    public record ApplicationRequestSearchModel : BaseSearchModel
    {
        [NopResourceDisplayName("Plugin.Widgets.KonnectAll.Features.ApplicationRequest.SearchFirstName")]
        public string SearchFirstName { get; set; }

        [NopResourceDisplayName("Plugin.Widgets.KonnectAll.Features.ApplicationRequest.SearchLastName")]
        public string SearchLastName { get; set; }

        [NopResourceDisplayName("Plugin.Widgets.KonnectAll.Features.ApplicationRequest.SearchEmail")]
        public string SearchEmail { get; set; }

        [NopResourceDisplayName("Plugin.Widgets.KonnectAll.Features.ApplicationRequest.SearchPhone")]
        public string SearchPhone { get; set; }
    }
}
