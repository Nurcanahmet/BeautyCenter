using Microsoft.AspNetCore.Mvc;

namespace Guzellik.Controllers
{
    public class Hakkimizda : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
