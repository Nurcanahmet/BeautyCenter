using Microsoft.AspNetCore.Mvc;

namespace BeautyCenter.Controllers
{
    public class HizmetlerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
