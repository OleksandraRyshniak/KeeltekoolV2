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
                .Take(20)
                .OrderBy(x => x.LastName);
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

    }
}
