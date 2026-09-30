using Keeltekool_2.Core.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Keeltekool_2.Controllers
{
    public class AccountsController : Controller
    {
        private readonly UserManager<ApplicationUser> _
        public IActionResult Index()
        {
            return View();
        }
    }
}
