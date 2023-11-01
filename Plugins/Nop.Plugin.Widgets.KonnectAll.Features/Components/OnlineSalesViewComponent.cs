using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Widgets.KonnectAll.Features.Areas.Admin.Factories;
using Nop.Plugin.Widgets.KonnectAll.Features.Services;
using Nop.Web.Framework.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.KonnectAll.Features.Components
{
    /// <summary>
    /// Represents Online sales view component
    /// </summary>
    [ViewComponent(Name = "OnlineSales")]
    public class OnlineSalesViewComponent : NopViewComponent
    {
        #region Fields
        private readonly IKonnectAllModelFactory _konnectAllModelFactory;
        #endregion

        #region Ctor
        public OnlineSalesViewComponent(IKonnectAllModelFactory konnectAllModelFactory) 
        {
            _konnectAllModelFactory = konnectAllModelFactory;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Invoke view component
        /// </summary>
        /// <param name="widgetZone">Widget zone name</param>
        /// <param name="additionalData">Additional data</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the view component result
        /// </returns>
        public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
        {
            if (string.IsNullOrEmpty(widgetZone))
                return Content("");

            var model = await _konnectAllModelFactory.PreparePublicInfoModelAsync();
            
            if (model == null)
                return Content("");

            return View(model);
        }
        #endregion
    }
}
