using F1_Fantasy_liga.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace F1_Fantasy_liga.Controllers
{
    [Route("fantasy-leagues")]
    public class FantasyLeaguesController : Controller
    {
        private readonly F1DbContext _db;

        public FantasyLeaguesController(F1DbContext db)
        {
            _db = db;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            var fantasyLeagues = _db.FantasyLeagues.ToList();
            return View(fantasyLeagues);
        }

        [HttpGet("{id:int}")]
        public IActionResult Details(int id)
        {
            var fantasyLeague = _db.FantasyLeagues
                .Include(fl => fl.FantasyTeams)
                .ThenInclude(ft => ft.User)
                .Include(fl => fl.FantasyTeams)
                .ThenInclude(ft => ft.Drivers)
                .ThenInclude(d => d.RaceResults)
                .Include(fl => fl.FantasyTeams)
                .ThenInclude(ft => ft.Constructor)
                .ThenInclude(c => c.Drivers)
                .ThenInclude(d => d.RaceResults)
                .FirstOrDefault(fl => fl.Id == id);
            if (fantasyLeague is null)
            {
                return NotFound();
            }

            return View(fantasyLeague);
        }
    }
}