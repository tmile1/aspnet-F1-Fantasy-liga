using F1_Fantasy_liga.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace F1_Fantasy_liga.Controllers
{
    [Route("race-results")]
    public class RaceResultsController : Controller
    {
        private readonly F1DbContext _db;

        public RaceResultsController(F1DbContext db)
        {
            _db = db;
        }

        [HttpGet("")]
        public IActionResult Index(string? raceSearch)
        {
            var raceResultsQuery = _db.RaceResults
                .Include(r => r.Race)
                .Include(r => r.Driver)
                .ThenInclude(d => d.Constructor)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(raceSearch))
            {
                raceResultsQuery = raceResultsQuery
                    .Where(r => r.Race != null && r.Race.Name.Contains(raceSearch));
            }

            ViewData["RaceSearch"] = raceSearch;
            return View(raceResultsQuery.ToList());
        }

        [HttpGet("{id:int}")]
        public IActionResult Details(int id)
        {
            var raceResult = _db.RaceResults
                .Include(r => r.Race)
                .Include(r => r.Driver)
                .ThenInclude(d => d.Constructor)
                .FirstOrDefault(r => r.Id == id);
            if (raceResult is null)
            {
                return NotFound();
            }

            return View(raceResult);
        }
    }
}