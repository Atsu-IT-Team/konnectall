using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Core.Domain.News;
using Nop.Web.Framework.Components;

namespace Nop.Web.Components
{
    public class OnlineSalesViewComponent : NopViewComponent
    {
        public OnlineSalesViewComponent()
        {

        }

        public IViewComponentResult Invoke(int currentCategoryId, int currentProductId)
        {
            return View();
        }
    }
}
