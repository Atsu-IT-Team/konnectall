using Microsoft.AspNetCore.Mvc.Razor;
using Nop.Core.Infrastructure;
using Nop.Web.Framework;
using Nop.Web.Framework.Themes;
using System.Collections.Generic;
using System.Linq;

namespace Nop.Plugin.Widgets.KonnectAll.Features.Infrastructure
{
    public class OnlineSalesViewLocationExpander : IViewLocationExpander
    {
        private const string THEME_KEY = "nop.themename";

        /// <summary>
        /// Invoked by a Microsoft.AspNetCore.Mvc.Razor.RazorViewEngine to determine potential locations for a view.
        /// </summary>
        /// <param name="context">Context</param>
        /// <param name="viewLocations">View locations</param>
        /// <returns>View locations</returns>
        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            if (context.Values.TryGetValue(THEME_KEY, out string theme))
            {
                viewLocations = new[] {
                        $"~/Plugins/Widgets.KonnectAll/Themes/{theme}/Views/{{1}}/{{0}}.cshtml",
                        $"~/Plugins/Widgets.KonnectAll/Themes/{theme}/Views/Shared/{{0}}.cshtml",
                        $"~/Plugins/Widgets.KonnectAll/Views/{{1}}/{{0}}.cshtml",
                        $"~/Plugins/Widgets.KonnectAll/Views/Shared/{{0}}.cshtml",
                    }
                    .Concat(viewLocations);
            }

            // check for area (admin)
            if (context.AreaName != null)
            {
                if (context.AreaName.Equals(AreaNames.Admin))
                {
                    viewLocations = new[] {
                        $"/Plugins/Widgets.KonnectAll/Areas/{AreaNames.Admin}/Views/{{1}}/{{0}}.cshtml",
                        $"/Plugins/Widgets.KonnectAll/Areas/{AreaNames.Admin}/Views/Shared/{{0}}.cshtml",

                    }.Concat(viewLocations);

                }
            }
            return viewLocations;
        }


        /// <summary>
        /// Invoked by a Microsoft.AspNetCore.Mvc.Razor.RazorViewEngine to determine the
        /// values that would be consumed by this instance of Microsoft.AspNetCore.Mvc.Razor.IViewLocationExpander.
        /// The calculated values are used to determine if the view location has changed since the last time it was located.
        /// </summary>
        /// <param name="context">Context</param>
        public void PopulateValues(ViewLocationExpanderContext context)
        {
            //no need to add the themeable view locations at all as the administration should not be themeable anyway
            if (context.AreaName?.Equals(AreaNames.Admin) ?? false)
                return;

            context.Values[THEME_KEY] = EngineContext.Current.Resolve<IThemeContext>().GetWorkingThemeNameAsync().Result;
        }
    }
}
