using F1_Fantasy_liga.Data;
using F1_Fantasy_liga.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            var constructors = BuildConstructorsQuery(null).ToList();
            return View(constructors);
        }

        [HttpGet("search")]
        public IActionResult Search(string? term)
        {
            var constructors = BuildConstructorsQuery(term).ToList();
            return PartialView("_ConstructorsCards", constructors);
        }

        [HttpGet("create")]
        public IActionResult Create()
        {
            return View(new Constructor());
        }

        [HttpGet("autocomplete")]
        public IActionResult Autocomplete(string? term)
        {
            var results = _db.Constructors
                .Where(c => c.IsDeleted == false)
                .Where(c => string.IsNullOrWhiteSpace(term) || c.Name.Contains(term))
                .OrderBy(c => c.Name)
                .Select(c => new { id = c.Id, label = c.Name })
                .Take(20)
                .ToList();

            return Json(results);
        }

        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Name,Nationality,FoundedDate")] Constructor model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.IsDeleted = false;
            model.DeletedAt = null;

            _db.Constructors.Add(model);
            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("{id:int}")]
        public IActionResult Details(int id)
        {
            var constructor = _db.Constructors
                .Include(c => c.Drivers.Where(d => d.IsDeleted == false))
                .ThenInclude(d => d.RaceResults.Where(rr => rr.IsDeleted == false && rr.Race != null && rr.Race.IsDeleted == false))
                .FirstOrDefault(c => c.Id == id && c.IsDeleted == false);
            if (constructor is null)
            {
                return NotFound();
            }

            return View(constructor);
        }

        [HttpGet("edit/{id:int}")]
        public IActionResult Edit(int id)
        {
            var constructor = _db.Constructors.FirstOrDefault(c => c.Id == id && c.IsDeleted == false);
            if (constructor is null)
            {
                return NotFound();
            }

            return View(constructor);
        }

        [HttpPost("edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Name,Nationality,FoundedDate")] Constructor model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var constructor = _db.Constructors.FirstOrDefault(c => c.Id == id && c.IsDeleted == false);
            if (constructor is null)
            {
                return NotFound();
            }

            constructor.Name = model.Name;
            constructor.Nationality = model.Nationality;
            constructor.FoundedDate = model.FoundedDate;

            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost("delete/{id:int}")]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var constructor = _db.Constructors.FirstOrDefault(c => c.Id == id && c.IsDeleted == false);
            if (constructor is null)
            {
                return NotFound();
            }

            constructor.IsDeleted = true;
            constructor.DeletedAt = DateTime.UtcNow;
            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        private IQueryable<Constructor> BuildConstructorsQuery(string? term)
        {
            var query = _db.Constructors
                .Where(c => c.IsDeleted == false)
                .Include(c => c.Drivers.Where(d => d.IsDeleted == false))
                .ThenInclude(d => d.RaceResults.Where(rr => rr.IsDeleted == false && rr.Race != null && rr.Race.IsDeleted == false))
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(term))
            {
                query = query.Where(c => c.Name.Contains(term));
            }

            return query;
        }
    }
}