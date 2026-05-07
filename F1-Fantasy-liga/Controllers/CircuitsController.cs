using F1_Fantasy_liga.Data;
using Microsoft.AspNetCore.Mvc;

namespace F1_Fantasy_liga.Controllers
{
    [Route("circuits")]
    public class CircuitsController : Controller
    {
        private readonly F1DbContext _db;

        public CircuitsController(F1DbContext db)
        {
            _db = db;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            var circuits = _db.Circuits.ToList();
            return View(circuits);
        }

        [HttpGet("{id:int}")]
        public IActionResult Details(int id)
        {
            var circuit = _db.Circuits.FirstOrDefault(c => c.Id == id);
            if (circuit is null)
            {
                return NotFound();
            }

            return View(circuit);
        }
    }
}