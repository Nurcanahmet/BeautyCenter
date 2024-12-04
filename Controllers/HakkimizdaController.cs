using Microsoft.AspNetCore.Mvc;

namespace BeautyCenter.Controllers
{
    public class HakkimizdaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
