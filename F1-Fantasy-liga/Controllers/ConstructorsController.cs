using F1_Fantasy_liga.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace F1_Fantasy_liga.Controllers
{
    [Route("constructors")]
    public class ConstructorsController : Controller
    {
        private readonly F1DbContext _db;

        public ConstructorsController(F1DbContext db)
        {
            _db = db;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            var constructors = _db.Constructors
                .Include(c => c.Drivers)
                .ThenInclude(d => d.RaceResults)
                .ToList();
            return View(constructors);
        }

        [HttpGet("{id:int}")]
        public IActionResult Details(int id)
        {
            var constructor = _db.Constructors
                .Include(c => c.Drivers)
                .ThenInclude(d => d.RaceResults)
                .FirstOrDefault(c => c.Id == id);
            if (constructor is null)
            {
                return NotFound();
            }

            return View(constructor);
        }
    }
}