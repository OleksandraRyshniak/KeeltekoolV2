using Keeltekool_2.Core.ServiceInterface;
using Microsoft.AspNetCore.Mvc;

namespace Keeltekool_2.Controllers
{
    public class StudentsController : Controller
    {
        private readonly IStudentsServices _studentsServices;

        public StudentsController(IStudentsServices studentsServices)
        {
            _studentsServices = studentsServices;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
