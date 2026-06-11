using F1_Fantasy_liga.Data;
using F1_Fantasy_liga.Models;
using F1_Fantasy_liga.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace F1_Fantasy_liga.Controllers
{
    [ApiController]
    [Route("api/fantasy-leagues")]
    public class FantasyLeaguesApiController : ControllerBase
    {
        private readonly F1DbContext _db;

        public FantasyLeaguesApiController(F1DbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public ActionResult<IEnumerable<FantasyLeagueDTO>> GetAll([FromQuery] string? search)
        {
            var query = _db.FantasyLeagues
                .Where(fl => fl.IsDeleted == false)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(fl => fl.Name.Contains(search));
            }

            var leagues = query
                .OrderBy(fl => fl.StartDate)
                .ThenBy(fl => fl.Name)
                .ToList()
                .Select(ToDTO)
                .ToList();

            return Ok(leagues);
        }

        [HttpGet("{id:int}")]
        public ActionResult<FantasyLeagueDTO> GetById(int id)
        {
            var league = _db.FantasyLeagues.FirstOrDefault(fl => fl.Id == id && fl.IsDeleted == false);
            if (league is null)
            {
                return NotFound();
            }

            return Ok(ToDTO(league));
        }

        [HttpPost]
        public ActionResult<FantasyLeagueDTO> Create([FromBody] FantasyLeagueCreateDTO model)
        {
            ValidateDates(model.StartDate, model.EndDate);

            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var league = new FantasyLeague
            {
                Name = model.Name,
                Description = model.Description,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                LeagueType = model.LeagueType,
                IsDeleted = false,
                DeletedAt = null
            };

            _db.FantasyLeagues.Add(league);
            _db.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = league.Id }, ToDTO(league));
        }

        [HttpPut("{id:int}")]
        public IActionResult Update(int id, [FromBody] FantasyLeagueUpdateDTO model)
        {
            var league = _db.FantasyLeagues.FirstOrDefault(fl => fl.Id == id && fl.IsDeleted == false);
            if (league is null)
            {
                return NotFound();
            }

            ValidateDates(model.StartDate, model.EndDate);

            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            league.Name = model.Name;
            league.Description = model.Description;
            league.StartDate = model.StartDate;
            league.EndDate = model.EndDate;
            league.LeagueType = model.LeagueType;

            _db.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var league = _db.FantasyLeagues.FirstOrDefault(fl => fl.Id == id && fl.IsDeleted == false);
            if (league is null)
            {
                return NotFound();
            }

            league.IsDeleted = true;
            league.DeletedAt = DateTime.UtcNow;
            _db.SaveChanges();

            return NoContent();
        }

        private void ValidateDates(DateTime startDate, DateTime endDate)
        {
            if (endDate < startDate)
            {
                ModelState.AddModelError(nameof(FantasyLeagueCreateDTO.EndDate), "End date cannot be before start date.");
            }
        }

        private static FantasyLeagueDTO ToDTO(FantasyLeague league)
        {
            return new FantasyLeagueDTO
            {
                Id = league.Id,
                Name = league.Name,
                Description = league.Description,
                StartDate = league.StartDate,
                EndDate = league.EndDate,
                LeagueType = league.LeagueType
            };
        }
    }
}
