using Keeltekool_2.Core.Domain;
using Keeltekool_2.Core.DTO;
using Keeltekool_2.Core.ServiceInterface;
using Keeltekool_2.Models.Accounts;
using MailKit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Keeltekool_2.Controllers
{
    [Authorize]
    public class AccountsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailingServices _emailServices;

        public AccountsController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailingServices emailServices)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailServices = emailServices;
        }

        // Sisukord:
        //
        // Registreerimine
        // Sisselogimine
        // 2fa
        // Kontotaaste
        // Kustutamine

        public IActionResult Index()
        {
            return View();
        }

        /*     R E G I S T R E E R I M I N E     */

        /// <summary>
        /// Viib kasutaja registreerimisvaatesse
        /// </summary>
        /// <returns>IActionResult</returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View(); //tagastab vaate
        }

        /// <summary>
        /// Registreerib uue kasutaja
        /// </summary>
        /// <param name="vm">Registreerimise vormi andmed</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = vm.Email,
                    Name = vm.Name,
                    Email = vm.Email,
                    Placeholder = vm.PlaceHolder,
                };

                var result = await _userManager.CreateAsync(user, vm.Password);

                if (result.Succeeded)
                {
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                    var confirmationLink = Url.Action(
                        "ConfirmEmail", "Accounts",
                        new { userId = user.Id, token = token },
                        Request.Scheme);

                    EmailTokenDTO newsignup = new();
                    newsignup.Token = token;
                    newsignup.Body = $"Palun kinnita oma konto vajutades <a href=\"{confirmationLink}\">siia</a>";
                    newsignup.Subject = "Keeltekooli registreerimine";
                    newsignup.To = user.Email!;

                    if (_signInManager.IsSignedIn(User) && User.IsInRole("Admin"))
                    {
                        return RedirectToAction("ListUsers", "Administrations");
                    }

                    _emailServices.SendEmailToken(newsignup, token);

                    List<string> errordatas =
                        [
                        "Area", "Accounts",
                        "Issue", "Success",
                        "StatusMessage", "Registration Sucesss",
                        "ActedOn", $"{vm.Email}",
                        "CreatedAccountData", $"{vm.Email}\n{vm.PlaceHolder}\n[password hidden]\n[password hidden]"
                        ];
                    ViewBag.ErrorDatas = errordatas;
                    ViewBag.ErrorTitle = "You have successfully registered";
                    ViewBag.ErrorMessage = "Before you can log in, please confirm email from the link" +
                        "\nwe have emailed to your email address.";
                    return View("ConfirmationEmailMessage");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(vm);
        }

        /// <summary>
        /// Kinnitab kasutaja e-posti lingi kaudu
        /// </summary>
        /// <param name="userId">Kasutaja Id</param>
        /// <param name="token">Kinnitustoken</param>
        /// <returns>IActionResult</returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            return BadRequest("Email confirmation failed");
        }
    }
}