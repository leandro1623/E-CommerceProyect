using E_CommerceSite.Entities;
using E_CommerceSite.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace E_CommerceSite.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<UserCustomer> userManager;
        private readonly SignInManager<UserCustomer> signInManager;

        public AccountController(UserManager<UserCustomer> userManager, SignInManager<UserCustomer> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new UserCustomer() { Fullname = model.Fullname ,Email = model.Email, UserName = model.Email };

            var result = await userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await signInManager.PasswordSignInAsync(user, model.Password , false, false);
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, result.ToString());
            
            return View(model);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await userManager.FindByEmailAsync(model.Email);

            if(user is null)
            {
                @TempData["Error"] = "Usuario o contraceña incorrecto";
                return View(model);
            }

            var result = await signInManager.PasswordSignInAsync(user!, model.Password, model.RememberMe, false);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            //errors
            @TempData["Error"] = "Usuario o contraceña incorrecto";

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}
