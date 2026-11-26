using Keeltekool_2.Core.Domain;
using Keeltekool_2.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Keeltekool_2.Controllers
{
    [ApiController]
    [Route("api/lecturers")]
    [Produces("application/json")]
    public class LecturersApiController : ControllerBase
    {
        private readonly Keeltekool_2Context _context;

        public LecturersApiController(Keeltekool_2Context context) =>
            _context = context;

        /// <summary>
        /// Gets all Lecturers.
        /// </summary>
        [HttpGet]
        public async Task<List<Lecturer>> Get() =>
            await _context.Lecturers.ToListAsync();

        /// <summary>
        /// Gets a specific Lecturer.
        /// </summary>
        /// <param name="id"></param>
        [HttpGet("{id}")]
        public async Task<ActionResult<Lecturer>> Get(Guid id)
        {
            var item = await _context.Lecturers.FindAsync(id);

            if (item is null)
            {
                return NotFound();
            }

            return item;
        }

        /// <summary>
        /// Creates a Lecturer.
        /// </summary>
        /// <param name="item"></param>
        /// <returns>A newly created Lecturer</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/lecturers
        ///     {
        ///        "firstName": "Jaan",
        ///        "lastName": "Tamm",
        ///        "qualification": "Eesti keele õpetaja"
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">If the item is null</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(Lecturer item)
        {
            item.Id = Guid.NewGuid(); // id genereerib server, kliendi saadetud id-d ei kasutata
            _context.Lecturers.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = item.Id }, item);
        }

        /// <summary>
        /// Deletes a specific Lecturer.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var item = await _context.Lecturers.FindAsync(id);

            if (item is null)
            {
                return NotFound();
            }

            _context.Lecturers.Remove(item);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
