using Microsoft.AspNetCore.Mvc;

namespace Guzellik.Controllers
{
    public class Api : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
