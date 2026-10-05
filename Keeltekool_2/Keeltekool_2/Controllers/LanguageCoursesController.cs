using Keeltekool_2.Core.Domain;
using Keeltekool_2.Core.ServiceInterface;
using Keeltekool_2.Data;
using Keeltekool_2.Models.LanguageCourses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Keeltekool_2.Controllers
{
    public class LanguageCoursesController : Controller
    {
        private readonly Keeltekool_2Context _context;
        private readonly ILanguageCoursesServices _languageCoursesServices;

        public LanguageCoursesController(Keeltekool_2Context context, ILanguageCoursesServices languageCoursesServices)
        {
            _context = context;
            _languageCoursesServices = languageCoursesServices;
        }
        public IActionResult Index()
        {
            return View();
            //need to get all, not under test rn
        }

        [HttpGet]
        public IActionResult Create()
        {
            LanguageCourseViewModel vm = new();
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(LanguageCourseViewModel vm)
        {
            //kontrollime et vm ei oleks null
            if (vm == null)
            {
                return RedirectToAction("Error", "Home");
            }
            //kontrollime et vmi modelstate on õige
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Error", "Home");
            }
            //teeme uue DTO-objekti
            //asetame dtosse vmi andmed
            var dto = new LanguageCourseDTO()
            {
                Id = vm.Id,
                Nimetus = vm.Nimetus,
                Keel = vm.Keel,
                Tase = vm.Tase,
                Kirjeldus = vm.Kirjeldus
            };
            //teostatakse päring teenusele
            var result = await _languageCoursesServices.Create(dto);
            //teenus peab objekti tagastama
            //kontrollime kas tagastatud objekt on null
            if (result == null)
            {
                //  kui on, suuname vealehele
                return RedirectToAction("Error", "Home");
            }
            else
            {
                //  kui ei, suuname tagasi indeksisse
                return RedirectToAction(nameof(Index));
            }
        }

    }
}