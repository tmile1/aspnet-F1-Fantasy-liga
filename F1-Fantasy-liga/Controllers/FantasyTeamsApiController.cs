using F1_Fantasy_liga.Data;
using F1_Fantasy_liga.Models;
using F1_Fantasy_liga.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace F1_Fantasy_liga.Controllers
{
    [ApiController]
    [Route("api/fantasy-teams")]
    public class FantasyTeamsApiController : ControllerBase
    {
        private readonly F1DbContext _db;

        public FantasyTeamsApiController(F1DbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public ActionResult<IEnumerable<FantasyTeamDTO>> GetAll([FromQuery] string? search)
        {
            var query = _db.FantasyTeams
                .Include(ft => ft.User)
                .Include(ft => ft.Constructor)
                .Include(ft => ft.FantasyLeague)
                .Where(ft => ft.IsDeleted == false
                    && ft.User != null
                    && ft.User.IsDeleted == false
                    && ft.FantasyLeague != null
                    && ft.FantasyLeague.IsDeleted == false)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(ft => ft.Name.Contains(search));
            }

            var teams = query
                .OrderBy(ft => ft.Name)
                .ToList()
                .Select(ToDTO)
                .ToList();

            return Ok(teams);
        }

        [HttpGet("{id:int}")]
        public ActionResult<FantasyTeamDTO> GetById(int id)
        {
            var team = _db.FantasyTeams
                .Include(ft => ft.User)
                .Include(ft => ft.Constructor)
                .Include(ft => ft.FantasyLeague)
                .FirstOrDefault(ft => ft.Id == id
                    && ft.IsDeleted == false
                    && ft.User != null
                    && ft.User.IsDeleted == false
                    && ft.FantasyLeague != null
                    && ft.FantasyLeague.IsDeleted == false);

            if (team is null)
            {
                return NotFound();
            }

            return Ok(ToDTO(team));
        }

        [HttpPost]
        public ActionResult<FantasyTeamDTO> Create([FromBody] FantasyTeamCreateDTO model)
        {
            ValidateFantasyTeamSelections(model);

            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var team = new FantasyTeam
            {
                Name = model.Name,
                Budget = model.Budget,
                UserId = model.UserId,
                ConstructorId = model.ConstructorId,
                FantasyLeagueId = model.FantasyLeagueId,
                IsDeleted = false,
                DeletedAt = null
            };

            _db.FantasyTeams.Add(team);
            _db.SaveChanges();

            var createdTeam = _db.FantasyTeams
                .Include(ft => ft.User)
                .Include(ft => ft.Constructor)
                .Include(ft => ft.FantasyLeague)
                .First(ft => ft.Id == team.Id);

            return CreatedAtAction(nameof(GetById), new { id = team.Id }, ToDTO(createdTeam));
        }

        [HttpPut("{id:int}")]
        public IActionResult Update(int id, [FromBody] FantasyTeamUpdateDTO model)
        {
            var team = _db.FantasyTeams.FirstOrDefault(ft => ft.Id == id && ft.IsDeleted == false);
            if (team is null)
            {
                return NotFound();
            }

            ValidateFantasyTeamSelections(model);

            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            team.Name = model.Name;
            team.Budget = model.Budget;
            team.UserId = model.UserId;
            team.ConstructorId = model.ConstructorId;
            team.FantasyLeagueId = model.FantasyLeagueId;

            _db.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var team = _db.FantasyTeams.FirstOrDefault(ft => ft.Id == id && ft.IsDeleted == false);
            if (team is null)
            {
                return NotFound();
            }

            team.IsDeleted = true;
            team.DeletedAt = DateTime.UtcNow;
            _db.SaveChanges();

            return NoContent();
        }

        private void ValidateFantasyTeamSelections(FantasyTeamCreateDTO model)
        {
            if (string.IsNullOrWhiteSpace(model.UserId))
            {
                ModelState.AddModelError(nameof(FantasyTeamCreateDTO.UserId), "User is required.");
            }
            else
            {
                var userExists = _db.Users.Any(u => u.Id == model.UserId && u.IsDeleted == false);
                if (!userExists)
                {
                    ModelState.AddModelError(nameof(FantasyTeamCreateDTO.UserId), "User not found.");
                }
            }

            if (model.ConstructorId <= 0)
            {
                ModelState.AddModelError(nameof(FantasyTeamCreateDTO.ConstructorId), "Constructor is required.");
            }
            else
            {
                var constructorExists = _db.Constructors.Any(c => c.Id == model.ConstructorId && c.IsDeleted == false);
                if (!constructorExists)
                {
                    ModelState.AddModelError(nameof(FantasyTeamCreateDTO.ConstructorId), "Constructor not found.");
                }
            }

            if (model.FantasyLeagueId <= 0)
            {
                ModelState.AddModelError(nameof(FantasyTeamCreateDTO.FantasyLeagueId), "Fantasy league is required.");
            }
            else
            {
                var leagueExists = _db.FantasyLeagues.Any(fl => fl.Id == model.FantasyLeagueId && fl.IsDeleted == false);
                if (!leagueExists)
                {
                    ModelState.AddModelError(nameof(FantasyTeamCreateDTO.FantasyLeagueId), "Fantasy league not found.");
                }
            }
        }

        private void ValidateFantasyTeamSelections(FantasyTeamUpdateDTO model)
        {
            if (string.IsNullOrWhiteSpace(model.UserId))
            {
                ModelState.AddModelError(nameof(FantasyTeamUpdateDTO.UserId), "User is required.");
            }
            else
            {
                var userExists = _db.Users.Any(u => u.Id == model.UserId && u.IsDeleted == false);
                if (!userExists)
                {
                    ModelState.AddModelError(nameof(FantasyTeamUpdateDTO.UserId), "User not found.");
                }
            }

            if (model.ConstructorId <= 0)
            {
                ModelState.AddModelError(nameof(FantasyTeamUpdateDTO.ConstructorId), "Constructor is required.");
            }
            else
            {
                var constructorExists = _db.Constructors.Any(c => c.Id == model.ConstructorId && c.IsDeleted == false);
                if (!constructorExists)
                {
                    ModelState.AddModelError(nameof(FantasyTeamUpdateDTO.ConstructorId), "Constructor not found.");
                }
            }

            if (model.FantasyLeagueId <= 0)
            {
                ModelState.AddModelError(nameof(FantasyTeamUpdateDTO.FantasyLeagueId), "Fantasy league is required.");
            }
            else
            {
                var leagueExists = _db.FantasyLeagues.Any(fl => fl.Id == model.FantasyLeagueId && fl.IsDeleted == false);
                if (!leagueExists)
                {
                    ModelState.AddModelError(nameof(FantasyTeamUpdateDTO.FantasyLeagueId), "Fantasy league not found.");
                }
            }
        }

        private static FantasyTeamDTO ToDTO(FantasyTeam team)
        {
            return new FantasyTeamDTO
            {
                Id = team.Id,
                Name = team.Name,
                Budget = team.Budget,
                User = team.User == null
                    ? null
                    : new UserSummaryDTO
                    {
                        Id = team.User.Id,
                        Name = team.User.Name,
                        Surname = team.User.Surname
                    },
                Constructor = team.Constructor == null || team.Constructor.IsDeleted
                    ? null
                    : new ConstructorSummaryDTO
                    {
                        Id = team.Constructor.Id,
                        Name = team.Constructor.Name
                    },
                FantasyLeague = team.FantasyLeague == null
                    ? null
                    : new FantasyLeagueSummaryDTO
                    {
                        Id = team.FantasyLeague.Id,
                        Name = team.FantasyLeague.Name
                    }
            };
        }
    }
}
