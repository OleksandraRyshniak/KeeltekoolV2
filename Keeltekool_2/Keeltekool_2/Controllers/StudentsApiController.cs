using Keeltekool_2.Core.Domain;
using Keeltekool_2.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Keeltekool_2.Controllers
{
    [ApiController]
    [Route("api/students")]
    [Produces("application/json")]
    public class StudentsApiController : ControllerBase
    {
        private readonly Keeltekool_2Context _context;

        public StudentsApiController(Keeltekool_2Context context) =>
            _context = context;

        /// <summary>
        /// Gets all Students.
        /// </summary>
        [HttpGet]
        public async Task<List<Student>> Get() =>
            await _context.Students.ToListAsync();

        /// <summary>
        /// Gets a specific Student.
        /// </summary>
        /// <param name="id"></param>
        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> Get(Guid id)
        {
            var item = await _context.Students.FindAsync(id);

            if (item is null)
            {
                return NotFound();
            }

            return item;
        }

        /// <summary>
        /// Creates a Student.
        /// </summary>
        /// <param name="item"></param>
        /// <returns>A newly created Student</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/students
        ///     {
        ///        "firstName": "Mari",
        ///        "lastName": "Maasikas"
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">If the item is null</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(Student item)
        {
            item.Id = Guid.NewGuid(); // id genereerib server, kliendi saadetud id-d ei kasutata
            _context.Students.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = item.Id }, item);
        }

        /// <summary>
        /// Deletes a specific Student.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var item = await _context.Students.FindAsync(id);

            if (item is null)
            {
                return NotFound();
            }

            _context.Students.Remove(item);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
