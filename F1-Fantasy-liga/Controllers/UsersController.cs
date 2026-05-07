using F1_Fantasy_liga.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace F1_Fantasy_liga.Controllers
{
    [Route("users")]
    public class UsersController : Controller
    {
        private readonly F1DbContext _db;

        public UsersController(F1DbContext db)
        {
            _db = db;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            var users = _db.Users.ToList();
            return View(users);
        }

        [HttpGet("{id:int}")]
        public IActionResult Details(int id)
        {
            var user = _db.Users
                .Include(u => u.FantasyTeams)
                .ThenInclude(ft => ft.FantasyLeague)
                .Include(u => u.FantasyTeams)
                .ThenInclude(ft => ft.Drivers)
                .ThenInclude(d => d.RaceResults)
                .Include(u => u.FantasyTeams)
                .ThenInclude(ft => ft.Constructor)
                .ThenInclude(c => c.Drivers)
                .ThenInclude(d => d.RaceResults)
                .FirstOrDefault(u => u.Id == id);
            if (user is null)
            {
                return NotFound();
            }

            return View(user);
        }
    }
}