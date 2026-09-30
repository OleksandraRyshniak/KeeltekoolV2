using Keeltekool_2.Core.Domain;
using Keeltekool_2.Models.Accounts;
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
        //private readonly IEmailServices _emailServices;

        public AccountsController
           (
               UserManager<ApplicationUser> userManager,
               SignInManager<ApplicationUser> signInManager/*,
                IEmailServices emailServices*/
           )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            //_emailServices = emailServices;
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

                    var confirmationLink = Url.Action("ConfirmEmail", "Accounts", new { userId = user.Id, token = token }, Request.Scheme);

                    // TODO: saada kiri, kui IEmailServices ja EmailTokenDto on olemas
                    // EmailTokenDto newsignup = new();
                    // newsignup.Token = token;
                    // newsignup.Body = $"Please registrate your account by: <a href=\"{confirmationLink}\">clicking here</a>";
                    // newsignup.Subject = "CRUD registration";
                    // newsignup.To = user.Email;
                    // _emailServices.SendEmailToken(newsignup, token);

                    if (_signInManager.IsSignedIn(User) && User.IsInRole("Admin"))
                    {
                        return RedirectToAction("ListUsers", "Administrations");
                    }

                    List<string> errordatas =
                        [
                        "Area", "Accounts",
                        "Issue", "Success",
                        "StatusMessage", "Registration Sucesss",
                        "ActedOn", $"{vm.Email}",
                        "CreatedAccountData", $"{vm.Email}\n{vm.Name}\n[password hidden]\n[password hidden]"
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
    }
}