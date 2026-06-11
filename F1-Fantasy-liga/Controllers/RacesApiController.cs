using F1_Fantasy_liga.Data;
using F1_Fantasy_liga.Models;
using F1_Fantasy_liga.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace F1_Fantasy_liga.Controllers
{
    [ApiController]
    [Route("api/races")]
    public class RacesApiController : ControllerBase
    {
        private readonly F1DbContext _db;

        public RacesApiController(F1DbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public ActionResult<IEnumerable<RaceDTO>> GetAll([FromQuery] string? search)
        {
            var query = _db.Races
                .Include(r => r.Circuit)
                .Where(r => r.IsDeleted == false && r.Circuit != null && r.Circuit.IsDeleted == false)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(r => r.Name.Contains(search));
            }

            var races = query
                .OrderBy(r => r.RaceDate)
                .ThenBy(r => r.Name)
                .ToList()
                .Select(ToDTO)
                .ToList();

            return Ok(races);
        }

        [HttpGet("{id:int}")]
        public ActionResult<RaceDTO> GetById(int id)
        {
            var race = _db.Races
                .Include(r => r.Circuit)
                .FirstOrDefault(r => r.Id == id && r.IsDeleted == false && r.Circuit != null && r.Circuit.IsDeleted == false);

            if (race is null)
            {
                return NotFound();
            }

            return Ok(ToDTO(race));
        }

        [HttpPost]
        public ActionResult<RaceDTO> Create([FromBody] RaceCreateDTO model)
        {
            ValidateCircuit(model.CircuitId);

            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var race = new Race
            {
                Name = model.Name,
                RaceDate = model.RaceDate,
                CircuitId = model.CircuitId,
                IsDeleted = false,
                DeletedAt = null
            };

            _db.Races.Add(race);
            _db.SaveChanges();

            var createdRace = _db.Races
                .Include(r => r.Circuit)
                .First(r => r.Id == race.Id);

            return CreatedAtAction(nameof(GetById), new { id = race.Id }, ToDTO(createdRace));
        }

        [HttpPut("{id:int}")]
        public IActionResult Update(int id, [FromBody] RaceUpdateDTO model)
        {
            var race = _db.Races.FirstOrDefault(r => r.Id == id && r.IsDeleted == false);
            if (race is null)
            {
                return NotFound();
            }

            ValidateCircuit(model.CircuitId);

            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            race.Name = model.Name;
            race.RaceDate = model.RaceDate;
            race.CircuitId = model.CircuitId;

            _db.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var race = _db.Races.FirstOrDefault(r => r.Id == id && r.IsDeleted == false);
            if (race is null)
            {
                return NotFound();
            }

            race.IsDeleted = true;
            race.DeletedAt = DateTime.UtcNow;
            _db.SaveChanges();

            return NoContent();
        }

        private void ValidateCircuit(int circuitId)
        {
            if (circuitId <= 0)
            {
                ModelState.AddModelError(nameof(RaceCreateDTO.CircuitId), "Circuit is required.");
                return;
            }

            var exists = _db.Circuits.Any(c => c.Id == circuitId && c.IsDeleted == false);
            if (!exists)
            {
                ModelState.AddModelError(nameof(RaceCreateDTO.CircuitId), "Circuit not found.");
            }
        }

        private static RaceDTO ToDTO(Race race)
        {
            return new RaceDTO
            {
                Id = race.Id,
                Name = race.Name,
                RaceDate = race.RaceDate,
                Circuit = race.Circuit == null
                    ? null
                    : new CircuitSummaryDTO
                    {
                        Id = race.Circuit.Id,
                        Name = race.Circuit.Name
                    }
            };
        }
    }
}
