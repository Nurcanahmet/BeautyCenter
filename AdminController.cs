using Microsoft.AspNetCore.Mvc;

namespace Guzellik.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {


            // Admin girişi başarılıysa mesajı görüntüle
            ViewBag.AdminMessage = TempData["AdminMessage"];
            return View();
        }
    }
}
