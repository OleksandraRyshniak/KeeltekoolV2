using Keeltekool_2.Core.Domain;
using Keeltekool_2.Core.ServiceInterface;
using Keeltekool_2.Data;
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

    }  //ViewModel on vajalik kasutajale info kuvamiseks, ja sealt edasi controllerile andmiseks, Viewmodel    //erineb DTO-objektist selle võrra, et kõik kasutajale mittevajalikud andmed on sealt eemaldatud.    //Valikulised andmed mis kuuluvad ka ka hiljem kasutajatele esitamiseks siiski jäävad.    //See eraldatus tagab ka selle et kasutaja ei saa pahatahtlikult soovimatutele andmetele ligipääsu. The End.    //and they all lived happily ever after.    public class LanguageCourseViewModel    {        public Guid Id { get; set; }        public string Nimetus { get; set; }        public string Keel { get; set; }        public string Tase { get; set; }        public string Kirjeldus { get; set; }    }
}
