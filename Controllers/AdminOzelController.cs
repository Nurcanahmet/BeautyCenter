using Microsoft.AspNetCore.Mvc;

namespace Guzellik.Controllers
{
    public class AdminOzelController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
