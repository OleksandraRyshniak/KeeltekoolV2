using Keeltekool_2.Core.ServiceInterface;
using Microsoft.AspNetCore.Mvc;
using Keeltekool_2.Data;
using Keeltekool_2.Models.Students;

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
    }
}
