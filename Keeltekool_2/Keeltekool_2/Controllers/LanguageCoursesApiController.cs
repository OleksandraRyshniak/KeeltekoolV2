using Keeltekool_2.Core.Domain;
using Keeltekool_2.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Keeltekool_2.Controllers
{
    [ApiController]
    [Route("api/languagecourses")]
    [Produces("application/json")]
    public class LanguageCoursesApiController : ControllerBase
    {
        private readonly Keeltekool_2Context _context;

        public LanguageCoursesApiController(Keeltekool_2Context context) =>
            _context = context;

        /// <summary>
        /// Gets all LanguageCourses.
        /// </summary>
        [HttpGet]
        public async Task<List<LanguageCourse>> Get() =>
            await _context.LanguageCourses.ToListAsync();

        /// <summary>
        /// Gets a specific LanguageCourse.
        /// </summary>
        /// <param name="id"></param>
        [HttpGet("{id}")]
        public async Task<ActionResult<LanguageCourse>> Get(Guid id)
        {
            var item = await _context.LanguageCourses.FindAsync(id);

            if (item is null)
            {
                return NotFound();
            }

            return item;
        }

        /// <summary>
        /// Creates a LanguageCourse.
        /// </summary>
        /// <param name="item"></param>
        /// <returns>A newly created LanguageCourse</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/languagecourses
        ///     {
        ///        "nimetus": "Eesti keel algajatele",
        ///        "keel": "Eesti",
        ///        "tase": "A1",
        ///        "kirjeldus": "Algtaseme kursus"
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">If the item is null</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(LanguageCourse item)
        {
            item.Id = Guid.NewGuid(); // id genereerib server, kliendi saadetud id-d ei kasutata
            _context.LanguageCourses.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = item.Id }, item);
        }

        /// <summary>
        /// Deletes a specific LanguageCourse.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var item = await _context.LanguageCourses.FindAsync(id);

            if (item is null)
            {
                return NotFound();
            }

            _context.LanguageCourses.Remove(item);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
