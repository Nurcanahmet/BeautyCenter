using Guzellik.Models;
using Guzellik.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Guzellik.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<AppUser> signInManager;
        private readonly UserManager<AppUser> userManager;

     

        // Admin bilgileri
       private const string AdminEmail = "seyma.orhan@ogr.sakarya.edu.tr";
        private const string AdminPassword = "B221210055";

        public AccountController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                // Admin kontrolü
                if (model.Username == AdminEmail && model.Password == AdminPassword)
                {
                    TempData["AdminMessage"] = "Admin Girişi Başarılı!";
                    // Admin sayfasına yönlendir
                    return RedirectToAction("Index", "Uzman");
                }

                // Normal kullanıcı girişi
                var result = await signInManager.PasswordSignInAsync(model.Username!, model.Password!, model.RememberMe, false);
                if (result.Succeeded)
                {
                    // Kullanıcı girişi başarılı
                    return RedirectToAction("Index", "Home");
                }

                // Giriş başarısız
                ModelState.AddModelError("", "Geçersiz giriş denemesi");
                return View(model);
            }

            return View(model);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new()
                {
                    Name = model.Name,
                    UserName = model.Email,
                    Email = model.Email,
                    Adress = model.Address
                };

                var result = await userManager.CreateAsync(user, model.Password!);
                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
