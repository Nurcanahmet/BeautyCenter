using Microsoft.AspNetCore.Mvc;

namespace Guzellik.Controllers
{
    public class Hizmetlerimiz : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
