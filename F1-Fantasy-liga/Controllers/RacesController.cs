using F1_Fantasy_liga.Data;
using F1_Fantasy_liga.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

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
            var races = BuildRacesQuery(null).ToList();
            return View(races);
        }

        [HttpGet("search")]
        public IActionResult Search(string? term)
        {
            var races = BuildRacesQuery(term).ToList();
            return PartialView("_RacesCards", races);
        }

        [HttpGet("create")]
        public IActionResult Create()
        {
            PopulateCircuits();
            return View(new Race());
        }

        [HttpGet("autocomplete")]
        public IActionResult Autocomplete(string? term)
        {
            var results = _db.Races
                .Where(r => r.IsDeleted == false && r.Circuit != null && r.Circuit.IsDeleted == false)
                .Where(r => string.IsNullOrWhiteSpace(term) || r.Name.Contains(term))
                .OrderBy(r => r.Name)
                .Select(r => new { id = r.Id, label = r.Name })
                .Take(20)
                .ToList();

            return Json(results);
        }

        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Name,RaceDate,CircuitId")] Race model)
        {
            if (model.CircuitId <= 0)
            {
                ModelState.AddModelError(nameof(Race.CircuitId), "Circuit is required.");
            }

            if (!ModelState.IsValid)
            {
                if (model.CircuitId > 0)
                {
                    ViewBag.CircuitName = _db.Circuits
                        .Where(c => c.Id == model.CircuitId && c.IsDeleted == false)
                        .Select(c => c.Name)
                        .FirstOrDefault();
                }

                PopulateCircuits(model.CircuitId);
                return View(model);
            }

            model.IsDeleted = false;
            model.DeletedAt = null;

            _db.Races.Add(model);
            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("{id:int}")]
        public IActionResult Details(int id)
        {
            var race = _db.Races
                .Include(r => r.Circuit)
                .Include(r => r.RaceResults.Where(rr => rr.IsDeleted == false && rr.Driver != null && rr.Driver.IsDeleted == false))
                .ThenInclude(rr => rr.Driver)
                .ThenInclude(d => d.Constructor)
                .FirstOrDefault(r => r.Id == id && r.IsDeleted == false && r.Circuit != null && r.Circuit.IsDeleted == false);
            if (race is null)
            {
                return NotFound();
            }

            return View(race);
        }

        [HttpGet("edit/{id:int}")]
        public IActionResult Edit(int id)
        {
            var race = _db.Races
                .Include(r => r.Circuit)
                .FirstOrDefault(r => r.Id == id && r.IsDeleted == false);
            if (race is null)
            {
                return NotFound();
            }

            PopulateCircuits(race.CircuitId);
            return View(race);
        }

        [HttpPost("edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Name,RaceDate,CircuitId")] Race model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            if (model.CircuitId <= 0)
            {
                ModelState.AddModelError(nameof(Race.CircuitId), "Circuit is required.");
            }

            if (!ModelState.IsValid)
            {
                if (model.CircuitId > 0)
                {
                    ViewBag.CircuitName = _db.Circuits
                        .Where(c => c.Id == model.CircuitId && c.IsDeleted == false)
                        .Select(c => c.Name)
                        .FirstOrDefault();
                }

                PopulateCircuits(model.CircuitId);
                return View(model);
            }

            var race = _db.Races.FirstOrDefault(r => r.Id == id && r.IsDeleted == false);
            if (race is null)
            {
                return NotFound();
            }

            race.Name = model.Name;
            race.RaceDate = model.RaceDate;
            race.CircuitId = model.CircuitId;

            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost("delete/{id:int}")]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var race = _db.Races.FirstOrDefault(r => r.Id == id && r.IsDeleted == false);
            if (race is null)
            {
                return NotFound();
            }

            race.IsDeleted = true;
            race.DeletedAt = DateTime.UtcNow;
            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        private void PopulateCircuits(int? selectedId = null)
        {
            var items = _db.Circuits
                .Where(c => c.IsDeleted == false)
                .OrderBy(c => c.Name)
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                })
                .ToList();

            items.Insert(0, new SelectListItem
            {
                Value = string.Empty,
                Text = "- select circuit -"
            });

            ViewBag.Circuits = new SelectList(items, "Value", "Text", selectedId?.ToString());
        }

        private IQueryable<Race> BuildRacesQuery(string? term)
        {
            var query = _db.Races
                .Include(r => r.Circuit)
                .Where(r => r.IsDeleted == false && r.Circuit != null && r.Circuit.IsDeleted == false)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(term))
            {
                query = query.Where(r => r.Name.Contains(term));
            }

            return query;
        }
    }
}