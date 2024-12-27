using Microsoft.AspNetCore.Mvc;
using Guzellik.Models;
using System.Linq;

namespace Guzellik.Controllers
{
    public class UzmanController : Controller
    {
        private static List<Uzman> uzmanlar = new List<Uzman>
        {
            new Uzman { Id = 1, Ad = "Hande", Soyad = "Erçel", Islem = "Saç-Makyaj-Tırnak", Maas = 17002 },
            new Uzman { Id = 2, Ad = "Dilan Çiçek", Soyad = "Deniz", Islem = "Saç-Makyaj-Tırnak", Maas = 17002 },
             new Uzman { Id = 3, Ad = "Elçin", Soyad = "Sangu", Islem = "Saç-Makyaj-Tırnak", Maas = 17002 },
            new Uzman { Id = 4, Ad = "Burcu", Soyad = "Biricik", Islem = "Saç-Makyaj-Tırnak", Maas = 17002 },
             new Uzman { Id = 5, Ad = "Aslı", Soyad = "Enver", Islem = "Saç-Makyaj-Tırnak", Maas = 17002 },
            new Uzman { Id = 6, Ad = "Farah Zeynep", Soyad = "Abdullah", Islem = "Saç-Makyaj-Tırnak", Maas = 17002 }


        };

        // Uzmanları listeleme
        public IActionResult Index()
        {
            return View(uzmanlar);  // Uzmanları listeleyen View'ı döndür
        }

        // Yeni Uzman eklemek için
        public IActionResult Create()
        {
            return View();  // Yeni uzman ekleme formunu döndür
        }

        [HttpPost]
        public IActionResult Create(Uzman uzman)
        {
            if (ModelState.IsValid)
            {
                uzman.Id = uzmanlar.Max(u => u.Id) + 1;  // Yeni ID atama
                uzmanlar.Add(uzman);  // Uzmanı listeye ekle
                return RedirectToAction("Index");  // Listele sayfasına yönlendir
            }
            return View(uzman);  // Hata durumunda aynı formu tekrar göster
        }

        // Uzman bilgilerini güncellemek için
        public IActionResult Edit(int id)
        {
            var uzman = uzmanlar.FirstOrDefault(u => u.Id == id);
            if (uzman == null)
            {
                return NotFound();  // Uzman bulunmazsa 404 döner
            }
            return View(uzman);  // Güncellenmek istenen uzmanı göster
        }

        [HttpPost]
        public IActionResult Edit(Uzman uzman)
        {
            if (ModelState.IsValid)
            {
                var mevcutUzman = uzmanlar.FirstOrDefault(u => u.Id == uzman.Id);
                if (mevcutUzman != null)
                {
                    mevcutUzman.Ad = uzman.Ad;
                    mevcutUzman.Soyad = uzman.Soyad;
                    mevcutUzman.Islem = uzman.Islem;
                    mevcutUzman.Maas = uzman.Maas;
                }
                return RedirectToAction("Index");  // Güncelleme sonrasında listele sayfasına dön
            }
            return View(uzman);  // Hata durumunda aynı sayfayı tekrar göster
        }



        // Uzman detaylarını gösterme
        public IActionResult Details(int id)
        {
            var uzman = uzmanlar.FirstOrDefault(u => u.Id == id);
            if (uzman == null)
            {
                return NotFound();  // Uzman bulunmazsa 404 döner
            }
            return View(uzman);  // Detayları gösteren View'ı döndür
        }

        // Uzman silme
        public IActionResult Delete(int id)
        {
            var uzman = uzmanlar.FirstOrDefault(u => u.Id == id);
            if (uzman != null)
            {
                uzmanlar.Remove(uzman);  // Uzmanı listeden sil
            }
            return RedirectToAction("Index");  // Silme işleminden sonra listele sayfasına yönlendir
        }
    }
}
