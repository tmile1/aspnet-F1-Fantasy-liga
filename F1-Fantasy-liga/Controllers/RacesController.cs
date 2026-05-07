using F1_Fantasy_liga.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace F1_Fantasy_liga.Controllers
{
    [Route("races")]
    public class RacesController : Controller
    {
        private readonly F1DbContext _db;

        public RacesController(F1DbContext db)
        {
            _db = db;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            var races = _db.Races
                .Include(r => r.Circuit)
                .ToList();
            return View(races);
        }

        [HttpGet("{id:int}")]
        public IActionResult Details(int id)
        {
            var race = _db.Races
                .Include(r => r.Circuit)
                .Include(r => r.RaceResults)
                .ThenInclude(rr => rr.Driver)
                .ThenInclude(d => d.Constructor)
                .FirstOrDefault(r => r.Id == id);
            if (race is null)
            {
                return NotFound();
            }

            return View(race);
        }
    }
}