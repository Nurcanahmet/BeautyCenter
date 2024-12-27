//using Python.Runtime;
using Microsoft.AspNetCore.Mvc;
  
namespace Guzellik.Controllers
{
    public class SacRengiController : Controller
    {
        // Anasayfa Görünümünü Göster 
        public IActionResult Index()
        {
            return View();
        }

        // Görseli al ve Python kodunu çalıştır
        [HttpPost]  
        public IActionResult AnalyzeHairColor(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                return View("Error", "Görsel yüklenmedi.");
            }

            // Görseli kaydetmek için bir yol oluştur
            string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder); // uploads klasörü yoksa oluşturuluyor
            }
            string filePath = Path.Combine(uploadsFolder, imageFile.FileName);

            // Görseli belirtilen dosyaya kaydet
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                imageFile.CopyTo(stream);
            }

            // Python kodunu çalıştırmak için Python.Runtime'ı kullan
            //using (Py.GIL()) // GIL (Global Interpreter Lock) ile Python çalışma ortamını başlat
            {
               // dynamic model = Py.Import("sac_rengi_tespiti"); // Python scriptinizi buraya ekleyin
               // dynamic result = model.sac_rengi_tespit_et(filePath); // Python fonksiyonunu çalıştır

                // Python'dan dönen sonucu string olarak alıyoruz
              //  string hairColor = result.ToString();

                // Sonucu "Result" görümünde kullanıcıya sunuyoruz
              //  ViewBag.HairColor = hairColor;
                return View("Result"); // Sonuç görünümüne yönlendirme
            }
        }
    }
}
