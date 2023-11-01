using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Widgets.KonnectAll.Features.Areas.Admin.Models.ApplicationRequest;
using Nop.Web.Framework.Components;

namespace Nop.Plugin.Widgets.KonnectAll.Features.Components
{
    /// <summary>
    /// Represents Application request view component
    /// </summary>
    [ViewComponent(Name = "ApplicationRequest")]
    public class ApplicationRequestViewComponent : NopViewComponent
    {
        /// <summary>
        /// Invoke view component
        /// </summary>
        /// <param name="widgetZone">Widget zone name</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the view component result
        /// </returns>
        public IViewComponentResult Invoke(string widgetZone, object additionalData)
        {
            if (string.IsNullOrEmpty(widgetZone))
                return Content("");

            var model = additionalData is ApplicationRequestModel arm ? arm : new ApplicationRequestModel();

            return View(model);
        }
    }
}
