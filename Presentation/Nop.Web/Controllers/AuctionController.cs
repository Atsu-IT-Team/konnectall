using Microsoft.AspNetCore.Mvc;

namespace Nop.Web.Controllers
{
    public class AuctionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
