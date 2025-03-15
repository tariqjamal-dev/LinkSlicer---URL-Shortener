using LinkSlicer.IServices;
using LinkSlicer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace LinkSlicer.Controllers
{
    public class AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IEmailSender emailSender, IConfiguration configuration, IUserUrlService userUrlService) : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
        private readonly IEmailSender _emailSender = emailSender;
        private readonly IConfiguration _configuration = configuration;
        private readonly IUserUrlService _userUrlService = userUrlService;

        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = new ApplicationUser { UserName = model.Email, Email = model.Email, RegistrationDate = DateTime.UtcNow };

            // Create the user first
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
                return View(model);
            }

            // Now generate the email confirmation token and send the email
            if (_configuration.GetValue<bool>("EnableEmailConfirmation"))
            {
                try
                {
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var confirmationLink = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, token }, Request.Scheme);

                    await _emailSender.SendEmailAsync(user.Email, "Confirm Your Email",
                        $"Please confirm your email by clicking <a href='{confirmationLink}'>here</a>");

                    return RedirectToAction("CheckEmail");
                }
                catch (Exception)
                {
                    // If email fails, delete the user to avoid an unverified account being stored
                    await _userManager.DeleteAsync(user);
                    ModelState.AddModelError(string.Empty, "Unable to process your request at the moment. Please try again later.");
                    return View(model);
                }
            }

            // If email confirmation is disabled, sign in the user directly
            await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction("Index", "UrlShortener");
        }


        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null) return BadRequest("Invalid email confirmation request.");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound("User not found.");

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                TempData["Message"] = "Your email has been successfully confirmed. You can now log in.";
                return RedirectToAction("Login", "Account");
            }

            return BadRequest("Email confirmation failed.");
        }


        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password");
                return View(model);
            }

            // Check email confirmation (if enabled)
            if (_configuration.GetValue<bool>("EnableEmailConfirmation") && !await _userManager.IsEmailConfirmedAsync(user))
            {
                ModelState.AddModelError(string.Empty, "Please confirm your email first.");
                return View(model);
            }

            await _userUrlService.MigrateAnonymousUrlsToUser(user, HttpContext);

            // Sign in the user
            await _signInManager.SignInAsync(user, isPersistent: model.RememberMe);
            return RedirectToAction("Home", "Dashboard");
        }


        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "UrlShortener");
        }

        public IActionResult CheckEmail()
        {
            TempData["Message"] = "A confirmation email has been sent. Please check your inbox.";
            return RedirectToAction("Login");
        }

    }
}