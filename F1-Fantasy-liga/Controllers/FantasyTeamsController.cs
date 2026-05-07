using F1_Fantasy_liga.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace F1_Fantasy_liga.Controllers
{
    [Route("fantasy-teams")]
    public class FantasyTeamsController : Controller
    {
        private readonly F1DbContext _db;

        public FantasyTeamsController(F1DbContext db)
        {
            _db = db;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            var fantasyTeams = _db.FantasyTeams
                .Include(ft => ft.User)
                .Include(ft => ft.FantasyLeague)
                .Include(ft => ft.Drivers)
                .ThenInclude(d => d.RaceResults)
                .Include(ft => ft.Constructor)
                .ThenInclude(c => c.Drivers)
                .ThenInclude(d => d.RaceResults)
                .ToList();
            return View(fantasyTeams);
        }

        [HttpGet("{id:int}")]
        public IActionResult Details(int id)
        {
            var fantasyTeam = _db.FantasyTeams
                .Include(ft => ft.User)
                .Include(ft => ft.FantasyLeague)
                .Include(ft => ft.Drivers)
                .ThenInclude(d => d.Constructor)
                .Include(ft => ft.Drivers)
                .ThenInclude(d => d.RaceResults)
                .Include(ft => ft.Constructor)
                .ThenInclude(c => c.Drivers)
                .ThenInclude(d => d.RaceResults)
                .FirstOrDefault(ft => ft.Id == id);
            if (fantasyTeam is null)
            {
                return NotFound();
            }

            return View(fantasyTeam);
        }
    }
}