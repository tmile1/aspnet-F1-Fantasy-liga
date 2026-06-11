using F1_Fantasy_liga.Data;
using F1_Fantasy_liga.Models;
using Microsoft.AspNetCore.Authorization;
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
            var circuits = BuildCircuitsQuery(null).ToList();
            return View(circuits);
        }

        [HttpGet("search")]
        public IActionResult Search(string? term)
        {
            var circuits = BuildCircuitsQuery(term).ToList();
            return PartialView("_CircuitsCards", circuits);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("create")]
        public IActionResult Create()
        {
            return View(new Circuit());
        }

        [HttpGet("autocomplete")]
        public IActionResult Autocomplete(string? term)
        {
            var results = _db.Circuits
                .Where(c => c.IsDeleted == false)
                .Where(c => string.IsNullOrWhiteSpace(term) || c.Name.Contains(term))
                .OrderBy(c => c.Name)
                .Select(c => new { id = c.Id, label = c.Name })
                .Take(20)
                .ToList();

            return Json(results);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Name,Country,City,Length,NumberOfLaps")] Circuit model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.IsDeleted = false;
            model.DeletedAt = null;

            _db.Circuits.Add(model);
            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
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

        [Authorize(Roles = "Admin")]
        [HttpGet("edit/{id:int}")]
        public IActionResult Edit(int id)
        {
            var circuit = _db.Circuits.FirstOrDefault(c => c.Id == id && c.IsDeleted == false);
            if (circuit is null)
            {
                return NotFound();
            }

            return View(circuit);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Name,Country,City,Length,NumberOfLaps")] Circuit model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var circuit = _db.Circuits.FirstOrDefault(c => c.Id == id && c.IsDeleted == false);
            if (circuit is null)
            {
                return NotFound();
            }

            circuit.Name = model.Name;
            circuit.Country = model.Country;
            circuit.City = model.City;
            circuit.Length = model.Length;
            circuit.NumberOfLaps = model.NumberOfLaps;

            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("delete/{id:int}")]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var circuit = _db.Circuits.FirstOrDefault(c => c.Id == id && c.IsDeleted == false);
            if (circuit is null)
            {
                return NotFound();
            }

            circuit.IsDeleted = true;
            circuit.DeletedAt = DateTime.UtcNow;
            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        private IQueryable<Circuit> BuildCircuitsQuery(string? term)
        {
            var query = _db.Circuits.Where(c => c.IsDeleted == false).AsQueryable();

            if (!string.IsNullOrWhiteSpace(term))
            {
                query = query.Where(c => c.Name.Contains(term));
            }

            return query;
        }
    }
}