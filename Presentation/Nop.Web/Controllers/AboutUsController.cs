using Microsoft.AspNetCore.Mvc;

namespace Nop.Web.Controllers
{
    public class AboutUsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
