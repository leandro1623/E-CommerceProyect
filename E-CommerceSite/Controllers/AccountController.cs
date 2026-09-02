using E_CommerceSite.Models;
using E_CommerceSite.Negocio.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace E_CommerceSite.Controllers
{
    public class AccountController : Controller
    {
        private readonly IManageUser userManager;
        private readonly ISignInManager signInManager;

        public AccountController(IManageUser userManager, ISignInManager signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await userManager.CreateAsync(model.Fullname, model.Email, model.Email,model.Password);

            if (result.Succeeded)
            {
                //await signInManager.GetSignInManager().PasswordSignInAsync(model.Email, model.Password , false, false);

                TempData["LoginMessageUserCreated"] = "Usuario registrado correctamente";
                return RedirectToAction(nameof(Login), "Account");
            }

            ModelState.AddModelError(string.Empty, result.ToString());
            
            return View(model);
        }


        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await userManager.GetUserManager().FindByEmailAsync(model.Email);

            if(user is null)
            {
                @TempData["Error"] = "Usuario o contraceña incorrecto";
                return View(model);
            }

            var result = await signInManager.GetSignInManager().PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

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
            await signInManager.GetSignInManager().SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}
