using Keeltekool_2.Core.ServiceInterface;
using Microsoft.AspNetCore.Mvc;
using Keeltekool_2.Data;
using Keeltekool_2.Models.Students;
using Keeltekool_2.Core.DTO;

namespace Keeltekool_2.Controllers
{
    public class StudentsController : Controller
    {
        private readonly Keeltekool_2Context _context;
        private readonly IStudentsServices _studentsServices;

        public StudentsController(IStudentsServices studentsServices, Keeltekool_2Context context)
        {
            _studentsServices = studentsServices;
            _context = context;
        }

        public IActionResult Index()
        {
            //võtame tudengite andmed
            var result = _context.Students
                .Select(x => new StudentIndexViewModel
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    UserID = x.UserID
                })
                //esmalt sorteerime, siis võtame 20 esimest
                .OrderBy(x => x.LastName)
                .Take(20);
            return View(result);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StudentCreateViewModel vm)
        {
            //kontrollime sisestatud andmeid
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            //loome uue tudengi
            StudentDTO dto = new StudentDTO();
            dto.FirstName = vm.FirstName;
            dto.LastName = vm.LastName;
            dto.UserID = vm.UserID;

            await _studentsServices.Create(dto);

            //suuname tagasi nimekirja
            return RedirectToAction(nameof(Index));
        }



        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            //otsime tudengi
            var student = await _studentsServices.DetailAsync(id);

            //kontrollime, kas tudeng on olemas
            if (student == null)
            {
                return NotFound();
            }

            //valmistame andmed ette
            var vm = new StudentDetailsViewModel
            {
                Id = student.Id,
                FirstName = student.FirstName,
                LastName = student.LastName,
                UserID = student.UserID,
                CreatedAt = student.CreatedAt,
                ModifiedAt = student.ModifiedAt
            };

            return View(vm);
        }


        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            //otsime tudengi
            var student = await _studentsServices.DetailAsync(id);

            //kontrollime, kas tudeng on olemas
            if (student == null)
            {
                return NotFound();
            }

            //valmistame andmed ette
            var vm = new StudentUpdateViewModel
            {
                Id = student.Id,
                FirstName = student.FirstName,
                LastName = student.LastName,
                UserID = student.UserID
            };

            return View(vm);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Guid id, StudentUpdateViewModel vm)
        {
            //kontrollime ID-d
            if (id != vm.Id)
            {
                return BadRequest();
            }

            //kontrollime sisestatud andmeid
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            //otsime tudengi
            var student = await _studentsServices.DetailAsync(id);

            if (student == null)
            {
                return NotFound();
            }

            //uuendame tudengi andmeid
            StudentDTO dto = new StudentDTO
            {
                Id = vm.Id,
                FirstName = vm.FirstName,
                LastName = vm.LastName,
                UserID = vm.UserID,
                CreatedAt = student.CreatedAt
            };

            await _studentsServices.Update(dto);

            //suuname tagasi nimekirja
            return RedirectToAction(nameof(Index));
        }



        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            //otsime tudengi
            var student = await _studentsServices.DetailAsync(id);

            //kontrollime, kas tudeng on olemas
            if (student == null)
            {
                return NotFound();
            }

            //valmistame andmed ette
            var vm = new StudentDetailsViewModel
            {
                Id = student.Id,
                FirstName = student.FirstName,
                LastName = student.LastName,
                UserID = student.UserID,
                CreatedAt = student.CreatedAt,
                ModifiedAt = student.ModifiedAt
            };

            return View(vm);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            //kontrollime, kas tudeng on olemas
            var student = await _studentsServices.DetailAsync(id);

            if (student == null)
            {
                return NotFound();
            }

            //kustutame tudengi
            await _studentsServices.Delete(id);

            //suuname tagasi nimekirja
            return RedirectToAction(nameof(Index));
        }

    }
}
