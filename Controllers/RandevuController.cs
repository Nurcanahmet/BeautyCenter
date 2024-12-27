using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Guzellik.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Guzellik.Controllers
{
    public class RandevuController : Controller
    {
        private readonly RandevuDbContext _context;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;



        // Uzman çalışma saatleri
        private readonly Dictionary<string, (TimeSpan baslangicSaati, TimeSpan bitisSaati)> uzmanSaatleri =
            new Dictionary<string, (TimeSpan, TimeSpan)>
            {
                { "Hande Erçel", (new TimeSpan(9, 0, 0), new TimeSpan(17, 0, 0)) },
                { "Aslı Enver", (new TimeSpan(10, 0, 0), new TimeSpan(18, 0, 0)) },
                { "Burcu Biricik", (new TimeSpan(11, 0, 0), new TimeSpan(19, 0, 0)) },
                { "Farah Zeynep Abdullah", (new TimeSpan(9, 0, 0), new TimeSpan(17, 0, 0)) },
                { "Dilan Çiçek Deniz", (new TimeSpan(10, 0, 0), new TimeSpan(18, 0, 0)) },
                { "Elçin Sangu", (new TimeSpan(11, 0, 0), new TimeSpan(19, 0, 0)) }
            };

        public RandevuController(RandevuDbContext context, SignInManager<AppUser> signInManager, UserManager<AppUser> userManager)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        // GET: Randevu
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            // Admin kontrolü
            if (user != null && user.UserName == "seyma.orhan@ogr.sakarya.edu.tr" && user.PasswordHash == "B221210055")
            {
                // Admin sayfasına yönlendir
                return RedirectToAction("AdminIndex", "Admin");
            }

            return View(await _context.Randevus.ToListAsync());
        }

        // GET: Randevu/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var randevu = await _context.Randevus
                .FirstOrDefaultAsync(m => m.RandevuID == id);
            if (randevu == null)
            {
                return NotFound();
            }

            return View(randevu);
        }

        // GET: Randevu/Create
        public IActionResult Create()
        {
            return View(new Randevu());
        }


        // Onayla
        [HttpPost]
        public async Task<IActionResult> Onayla(int id)
        {
            var randevu = await _context.Randevus.FindAsync(id);
            if (randevu == null)
            {
                return NotFound();
            }

            randevu.Onaylandi = true;
            _context.Update(randevu);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index)); // Onaylanan randevuların listelendiği sayfaya yönlendir
        }

        // Reddet
        [HttpPost]
        public async Task<IActionResult> Red(int id)
        {
            var randevu = await _context.Randevus.FindAsync(id);
            if (randevu == null)
            {
                return NotFound();
            }

            randevu.Onaylandi = false;
            _context.Update(randevu);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index)); // Randevu reddedildikten sonra listeyi göster
        }


        /// //////////////////////////////////////////// BURASI SAAT KONTROLÜÜÜ <summary>
        /// //////////////////////////////////////////// BURASI SAAT KONTROLÜÜÜ
        /// 

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RandevuID,MusteriAd,MusteriSoyad,UzmanAd,IslemAd,IslemSure,IslemUcret,IslemTarih")] Randevu randevu)
        {
            // Dükkanın açık olduğu saat aralığı
            var acilisSaati = new TimeSpan(9, 0, 0);  // 09:00
            var kapanisSaati = new TimeSpan(18, 0, 0); // 18:00

            // Seçilen randevu saati
            var secilenSaat = randevu.IslemTarih.TimeOfDay;

            // Seçilen saat dükkanın açık olduğu saat aralığındaysa, randevu almasına izin ver
            if (secilenSaat < acilisSaati || secilenSaat > kapanisSaati)
            {
                // Saat aralığı dışıysa, hata mesajı göster
                ModelState.AddModelError("", "Randevu saati dükkanın açık saatleri dışında.");
                return View(randevu);
            }

            // Aynı saatte aynı uzmana randevu olup olmadığını kontrol et
            var ayniSaatteRandevu = await _context.Randevus
                .FirstOrDefaultAsync(r => r.UzmanAd == randevu.UzmanAd && r.IslemTarih == randevu.IslemTarih);

            if (ayniSaatteRandevu != null)
            {
                // Eğer aynı saatte randevu varsa, hata mesajı göster
                ModelState.AddModelError("", "Bu saatte bu uzmana zaten bir randevu var.");
                return View(randevu);
            }

            if (ModelState.IsValid)
            {
                _context.Add(randevu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(randevu);
        }




        // GET: Randevu/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var randevu = await _context.Randevus.FindAsync(id);
            if (randevu == null)
            {
                return NotFound();
            }
            return View(randevu);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RandevuID,MusteriAd,MusteriSoyad,UzmanAd,IslemAd,IslemSure,IslemUcret,IslemTarih")] Randevu randevu)
        {
            if (id != randevu.RandevuID)
            {
                return NotFound();
            }

            // Dükkanın açık olduğu saat aralığı
            var acilisSaati = new TimeSpan(9, 0, 0);  // 09:00
            var kapanisSaati = new TimeSpan(18, 0, 0); // 18:00

            // Seçilen randevu saati
            var secilenSaat = randevu.IslemTarih.TimeOfDay;

            // Seçilen saat dükkanın açık olduğu saat aralığındaysa, randevu almasına izin ver
            if (secilenSaat < acilisSaati || secilenSaat > kapanisSaati)
            {
                // Saat aralığı dışıysa, hata mesajı göster
                ModelState.AddModelError("", "Randevu saati dükkanın açık saatleri dışında.");
                return View(randevu);
            }

            // Aynı saatte aynı uzmana randevu olup olmadığını kontrol et
            var ayniSaatteRandevu = await _context.Randevus
                .FirstOrDefaultAsync(r => r.UzmanAd == randevu.UzmanAd && r.IslemTarih == randevu.IslemTarih && r.RandevuID != id);

            if (ayniSaatteRandevu != null)
            {
                // Eğer aynı saatte randevu varsa, hata mesajı göster
                ModelState.AddModelError("", "Bu saatte bu uzmana zaten bir randevu var.");
                return View(randevu);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(randevu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RandevuExists(randevu.RandevuID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(randevu);
        }





        // GET: Randevu/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var randevu = await _context.Randevus
                .FirstOrDefaultAsync(m => m.RandevuID == id);
            if (randevu == null)
            {
                return NotFound();
            }

            return View(randevu);
        }

        // POST: Randevu/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var randevu = await _context.Randevus.FindAsync(id);
            if (randevu != null)
            {
                _context.Randevus.Remove(randevu);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RandevuExists(int id)
        {
            return _context.Randevus.Any(e => e.RandevuID == id);
        }
    }
}
