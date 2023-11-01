using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;
using System.Collections.Generic;

namespace Nop.Plugin.Widgets.KonnectAll.Features.Areas.Admin.Models.ApplicationRequest
{
    /// <summary>
    /// Represents a application request model
    /// </summary>
    public record ApplicationRequestModel : BaseNopEntityModel
    {
        public ApplicationRequestModel() 
        {
            Documents = new List<ApplicationOtherDocumentsModel>();
        }

        public string FirstName { get; set; }
        
        public string LastName { get; set; }

        [NopResourceDisplayName("Plugin.Widgets.KonnectAll.Features.ApplicationRequest.FullName")]
        public string FullName { get; set; }

        [NopResourceDisplayName("Plugin.Widgets.KonnectAll.Features.ApplicationRequest.Email")]
        public string Email { get; set; }

        [NopResourceDisplayName("Plugin.Widgets.KonnectAll.Features.ApplicationRequest.Phone")]
        public string Phone { get; set; }

        [NopResourceDisplayName("Plugin.Widgets.KonnectAll.Features.ApplicationRequest.Resume")]
        public string Resume { get; set; }
        public string ResumeLink { get; set; }

        [NopResourceDisplayName("Plugin.Widgets.KonnectAll.Features.ApplicationRequest.Documents")]
        public IList<ApplicationOtherDocumentsModel> Documents { get; set; }
    }

    public record ApplicationOtherDocumentsModel
    {
        public string Name { get; set; }
        public string DownloadLink { get; set; }
    }
}
