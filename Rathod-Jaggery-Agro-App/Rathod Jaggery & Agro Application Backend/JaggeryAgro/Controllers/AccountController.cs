using JaggeryAgro.Core.Entities; // ✅ use ApplicationUser, ApplicationRole
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using JaggeryAgro.Data.Models;

namespace JaggeryAgro.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;   // ✅ changed
        private readonly SignInManager<ApplicationUser> _signInManager; // ✅ changed
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

            if (result.Succeeded)
            {
                _logger.LogInformation("User logged in successfully at {Time}", DateTime.Now);
                return RedirectToAction("Index", "Dashboard");
            }

            _logger.LogWarning("Invalid login attempt at {Time}", DateTime.Now);
            ModelState.AddModelError("", "Invalid login attempt.");
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            // ✅ Use ApplicationUser instead of IdentityUser
            var user = new ApplicationUser { UserName = model.Email, Email = model.Email };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                _logger.LogInformation("User registered successfully at {Time}", DateTime.Now);

                await _userManager.AddToRoleAsync(user, model.Role);
                await _signInManager.SignInAsync(user, isPersistent: false);

                if (model.Role == "Admin")
                    return RedirectToAction("Index", "Labor");
                else
                    return RedirectToAction("Index", "Attendance");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            _logger.LogWarning("User registration failed at {Time}", DateTime.Now);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                return RedirectToAction("ForgotPasswordConfirmation");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callbackUrl = Url.Action("ResetPassword", "Account", new { token, email = model.Email }, Request.Scheme);

            Console.WriteLine("Reset Link: " + callbackUrl);

            _logger.LogInformation("Password reset token generated at {Time}", DateTime.Now);

            return RedirectToAction("ForgotPasswordConfirmation");
        }
        [HttpGet]
        public IActionResult ChooseLogin()
        {
            return View();
        }
    }
}

        //[HttpGet]
        //public IActionResult ChooseLogin()
        //{
        //    return View();
        //}
