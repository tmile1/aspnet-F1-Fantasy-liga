using F1_Fantasy_liga.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace F1_Fantasy_liga.Controllers
{
    [Route("drivers")]
    public class DriversController : Controller
    {
        private readonly F1DbContext _db;

        public DriversController(F1DbContext db)
        {
            _db = db;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            var drivers = _db.Drivers
                .Include(d => d.Constructor)
                .Include(d => d.RaceResults)
                .ToList();
            return View(drivers);
        }

        [HttpGet("{id:int}")]
        public IActionResult Details(int id)
        {
            var driver = _db.Drivers
                .Include(d => d.Constructor)
                .Include(d => d.RaceResults)
                .FirstOrDefault(d => d.Id == id);
            if (driver is null)
            {
                return NotFound();
            }

            return View(driver);
        }
    }
}