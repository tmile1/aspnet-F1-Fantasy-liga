using F1_Fantasy_liga.Data;
using F1_Fantasy_liga.Models;
using F1_Fantasy_liga.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace F1_Fantasy_liga.Controllers
{
    [ApiController]
    [Route("api/constructors")]
    public class ConstructorsApiController : ControllerBase
    {
        private readonly F1DbContext _db;

        public ConstructorsApiController(F1DbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ConstructorDTO>> GetAll([FromQuery] string? search)
        {
            var query = _db.Constructors
                .Where(c => c.IsDeleted == false)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(c => c.Name.Contains(search));
            }

            var constructors = query
                .OrderBy(c => c.Name)
                .ToList()
                .Select(ToDTO)
                .ToList();

            return Ok(constructors);
        }

        [HttpGet("{id:int}")]
        public ActionResult<ConstructorDTO> GetById(int id)
        {
            var constructor = _db.Constructors.FirstOrDefault(c => c.Id == id && c.IsDeleted == false);
            if (constructor is null)
            {
                return NotFound();
            }

            return Ok(ToDTO(constructor));
        }

        [HttpPost]
        public ActionResult<ConstructorDTO> Create([FromBody] ConstructorCreateDTO model)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var constructor = new Constructor
            {
                Name = model.Name,
                Nationality = model.Nationality,
                FoundedDate = model.FoundedDate,
                IsDeleted = false,
                DeletedAt = null
            };

            _db.Constructors.Add(constructor);
            _db.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = constructor.Id }, ToDTO(constructor));
        }

        [HttpPut("{id:int}")]
        public IActionResult Update(int id, [FromBody] ConstructorUpdateDTO model)
        {
            var constructor = _db.Constructors.FirstOrDefault(c => c.Id == id && c.IsDeleted == false);
            if (constructor is null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            constructor.Name = model.Name;
            constructor.Nationality = model.Nationality;
            constructor.FoundedDate = model.FoundedDate;

            _db.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var constructor = _db.Constructors.FirstOrDefault(c => c.Id == id && c.IsDeleted == false);
            if (constructor is null)
            {
                return NotFound();
            }

            constructor.IsDeleted = true;
            constructor.DeletedAt = DateTime.UtcNow;
            _db.SaveChanges();

            return NoContent();
        }

        private static ConstructorDTO ToDTO(Constructor constructor)
        {
            return new ConstructorDTO
            {
                Id = constructor.Id,
                Name = constructor.Name,
                Nationality = constructor.Nationality,
                FoundedDate = constructor.FoundedDate
            };
        }
    }
}
