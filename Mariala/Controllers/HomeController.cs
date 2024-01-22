using Microsoft.AspNetCore.Mvc;

namespace Mariala.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
