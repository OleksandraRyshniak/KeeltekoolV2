using Keeltekool_2.ApplicationServices.Services;
using Keeltekool_2.Core.ServiceInterface;
using Keeltekool_2.Data;
using Microsoft.AspNetCore.Mvc;

namespace Keeltekool_2.Controllers
{
    public class LecturersController : Controller
    {
        private readonly Keeltekool_2Context _context;
        private readonly ILecturersServices _lecturersServices;
        private readonly IFilesServices _filesServices;

        public LecturersController(
         Keeltekool_2Context context,
         ILecturersServices lecturersServices,
         IFilesServices filesServices)
        {
            _context = context;
            _lecturersServices = lecturersServices;
            _filesServices = filesServices;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
