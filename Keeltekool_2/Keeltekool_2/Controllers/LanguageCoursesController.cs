using Keeltekool_2.Core.Domain;
using Keeltekool_2.Core.DTO;
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
            ////gets everything
            //var result = _context.LanguageCourses.ToList();
            // get only some, with limited info
            var result = _context.LanguageCourses
                .Select(x => new LanguageCourseViewModel
                {
                    Id=x.Id,
                    Nimetus = x.Nimetus,
                    Keel = x.Keel,
                }).Take(20).OrderBy(x => x.Keel);
            return View(result);

        }

        [HttpGet]
        public IActionResult Create()
        {
            LanguageCourseCreateUpdateViewModel vm = new();
            return View("CreateUpdate", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(LanguageCourseCreateUpdateViewModel vm)
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

       
        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }
            var languageCourse = await _languageCoursesServices.DetailsAsync(id);
            if(languageCourse == null)
            {
                return NotFound();
            }
            var vm = new LanguageCourseViewModel()
            {};
            vm.Id = languageCourse.Id;
            vm.Kirjeldus = languageCourse.Kirjeldus;
            vm.Nimetus = languageCourse.Nimetus;
            vm.Keel = languageCourse.Keel;
            vm.Tase = languageCourse.Tase;
            return View(vm);

        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }
            var courseToUpdate = await _languageCoursesServices.DetailsAsync(id);
            if(courseToUpdate == null)
            {
                return NotFound();
            }

            var vm = new LanguageCourseCreateUpdateViewModel()
            {};
            vm.Id = courseToUpdate.Id;
            vm.Nimetus = courseToUpdate.Nimetus;
            vm.Keel = courseToUpdate.Keel;
            vm.Tase = courseToUpdate.Tase;
            vm.Kirjeldus = courseToUpdate.Kirjeldus;
            vm.CreatedAt = courseToUpdate.CreatedAt;
            vm.ModifiedAt = courseToUpdate.ModifiedAt;

            return View("CreateUpdate", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(LanguageCourseCreateUpdateViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View("CreateUpdate", vm);
            }

            var dto = new LanguageCourseDTO()
            {
                Id = vm.Id,
                Nimetus = vm.Nimetus,
                Keel = vm.Keel,
                Tase = vm.Tase,
                Kirjeldus = vm.Kirjeldus,
                CreatedAt = vm.CreatedAt,
                ModifiedAt = vm.ModifiedAt
            };

            var result = await _languageCoursesServices.Update(dto);

            if (result == null)
            {
                return RedirectToAction("Error", "Home");
            }

            return RedirectToAction(nameof(Index));
        }

    }
}