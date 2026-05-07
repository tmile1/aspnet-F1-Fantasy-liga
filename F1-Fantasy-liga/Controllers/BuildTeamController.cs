using F1_Fantasy_liga.Data;
using F1_Fantasy_liga.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace F1_Fantasy_liga.Controllers
{
    [Route("build-team")]
    public class BuildTeamController : Controller
    {
        private readonly F1DbContext _db;

        public BuildTeamController(F1DbContext db)
        {
            _db = db;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            var model = new BuildFantasyTeamViewModel
            {
                Drivers = _db.Drivers
                    .Include(d => d.Constructor)
                    .Include(d => d.RaceResults)
                    .ToList(),
                Constructors = _db.Constructors
                    .Include(c => c.Drivers)
                    .ThenInclude(d => d.RaceResults)
                    .ToList()
            };

            return View(model);
        }
    }
}