using F1_Fantasy_liga.Data;
using F1_Fantasy_liga.Models;
using F1_Fantasy_liga.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace F1_Fantasy_liga.Controllers
{
    [ApiController]
    [Route("api/circuits")]
    public class CircuitsApiController : ControllerBase
    {
        private readonly F1DbContext _db;

        public CircuitsApiController(F1DbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CircuitDTO>> GetAll([FromQuery] string? search)
        {
            var query = _db.Circuits
                .Where(c => c.IsDeleted == false)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(c => c.Name.Contains(search));
            }

            var circuits = query
                .OrderBy(c => c.Name)
                .ToList()
                .Select(ToDTO)
                .ToList();

            return Ok(circuits);
        }

        [HttpGet("{id:int}")]
        public ActionResult<CircuitDTO> GetById(int id)
        {
            var circuit = _db.Circuits.FirstOrDefault(c => c.Id == id && c.IsDeleted == false);
            if (circuit is null)
            {
                return NotFound();
            }

            return Ok(ToDTO(circuit));
        }

        [HttpPost]
        public ActionResult<CircuitDTO> Create([FromBody] CircuitCreateDTO model)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var circuit = new Circuit
            {
                Name = model.Name,
                Country = model.Country,
                City = model.City,
                Length = model.Length,
                NumberOfLaps = model.NumberOfLaps,
                IsDeleted = false,
                DeletedAt = null
            };

            _db.Circuits.Add(circuit);
            _db.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = circuit.Id }, ToDTO(circuit));
        }

        [HttpPut("{id:int}")]
        public IActionResult Update(int id, [FromBody] CircuitUpdateDTO model)
        {
            var circuit = _db.Circuits.FirstOrDefault(c => c.Id == id && c.IsDeleted == false);
            if (circuit is null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            circuit.Name = model.Name;
            circuit.Country = model.Country;
            circuit.City = model.City;
            circuit.Length = model.Length;
            circuit.NumberOfLaps = model.NumberOfLaps;

            _db.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var circuit = _db.Circuits.FirstOrDefault(c => c.Id == id && c.IsDeleted == false);
            if (circuit is null)
            {
                return NotFound();
            }

            circuit.IsDeleted = true;
            circuit.DeletedAt = DateTime.UtcNow;
            _db.SaveChanges();

            return NoContent();
        }

        private static CircuitDTO ToDTO(Circuit circuit)
        {
            return new CircuitDTO
            {
                Id = circuit.Id,
                Name = circuit.Name,
                Country = circuit.Country,
                City = circuit.City,
                Length = circuit.Length,
                NumberOfLaps = circuit.NumberOfLaps
            };
        }
    }
}
