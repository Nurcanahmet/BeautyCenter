using BeautyCenter.Models;
using Microsoft.AspNetCore.Mvc;
//using YourNamespace.Models;

namespace YourNamespace.Controllers
{
    public class RandevuController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(Randevu model)
        {
            if (ModelState.IsValid)
            {
                // Veritabanına kaydetme işlemi burada yapılır.
                // Örn: _dbContext.Randevular.Add(model); _dbContext.SaveChanges();

                TempData["Message"] = "Randevunuz başarıyla alındı!";
                return RedirectToAction("Success");
            }

            return View(model);
        }

        public IActionResult Success()
        {
            ViewBag.Message = TempData["Message"];
            return View();
        }
    }
}

