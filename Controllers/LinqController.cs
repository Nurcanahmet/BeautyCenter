using Microsoft.AspNetCore.Mvc;
using Guzellik.Models; // Model sınıfının bulunduğu namespace
using Guzellik.Migrations.RandevuDb; // DbContext sınıfının bulunduğu namespace
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Guzellik.Controllers
{
    public class LinqController : Controller
    {
        private readonly RandevuDbContext _context;

        public LinqController(RandevuDbContext context)
        {
            _context = context;
        }

        // İşleme Göre Randevular
        public IActionResult Index(string IslemAd)
        {
            var randevular = _context.Randevus  // Randevus DbSet adını doğru kullanıyoruz
                .Where(r => r.IslemAd.Contains(IslemAd))  // IslemAd'ı sorguluyoruz
                .ToList();

            return View(randevular);
        }
    }
}
