using F1_Fantasy_liga.Data;
using F1_Fantasy_liga.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

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
            var drivers = BuildDriversQuery(null).ToList();
            return View(drivers);
        }

        [HttpGet("search")]
        public IActionResult Search(string? term)
        {
            var drivers = BuildDriversQuery(term).ToList();
            return PartialView("_DriversCards", drivers);
        }

        [HttpGet("create")]
        public IActionResult Create()
        {
            PopulateConstructors();
            return View(new Driver());
        }

        [HttpGet("autocomplete")]
        public IActionResult Autocomplete(string? term)
        {
            var results = _db.Drivers
                .Where(d => d.IsDeleted == false && d.Constructor != null && d.Constructor.IsDeleted == false)
                .Where(d => string.IsNullOrWhiteSpace(term) || (d.Name + " " + d.Surname).Contains(term))
                .OrderBy(d => d.Name)
                .Select(d => new { id = d.Id, label = d.Name + " " + d.Surname })
                .Take(20)
                .ToList();

            return Json(results);
        }

        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Name,Surname,Number,Price,ConstructorId")] Driver model)
        {
            if (model.ConstructorId <= 0)
            {
                ModelState.AddModelError(nameof(Driver.ConstructorId), "Constructor is required.");
            }

            var numberTaken = _db.Drivers.Any(d => d.IsDeleted == false && d.Number == model.Number);
            if (numberTaken)
            {
                ModelState.AddModelError(nameof(Driver.Number), "Driver number is already taken.");
            }

            if (!ModelState.IsValid)
            {
                if (model.ConstructorId > 0)
                {
                    ViewBag.ConstructorName = _db.Constructors
                        .Where(c => c.Id == model.ConstructorId && c.IsDeleted == false)
                        .Select(c => c.Name)
                        .FirstOrDefault();
                }

                PopulateConstructors(model.ConstructorId);
                return View(model);
            }

            model.IsDeleted = false;
            model.DeletedAt = null;

            _db.Drivers.Add(model);
            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("edit/{id:int}")]
        public IActionResult Edit(int id)
        {
            var driver = _db.Drivers
                .Include(d => d.Constructor)
                .FirstOrDefault(d => d.Id == id && d.IsDeleted == false);
            if (driver is null)
            {
                return NotFound();
            }

            PopulateConstructors(driver.ConstructorId);
            return View(driver);
        }

        [HttpPost("edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Name,Surname,Number,Price,ConstructorId")] Driver model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            if (model.ConstructorId <= 0)
            {
                ModelState.AddModelError(nameof(Driver.ConstructorId), "Constructor is required.");
            }

            var numberTaken = _db.Drivers.Any(d => d.IsDeleted == false && d.Number == model.Number && d.Id != id);
            if (numberTaken)
            {
                ModelState.AddModelError(nameof(Driver.Number), "Driver number is already taken.");
            }

            if (!ModelState.IsValid)
            {
                if (model.ConstructorId > 0)
                {
                    ViewBag.ConstructorName = _db.Constructors
                        .Where(c => c.Id == model.ConstructorId && c.IsDeleted == false)
                        .Select(c => c.Name)
                        .FirstOrDefault();
                }

                PopulateConstructors(model.ConstructorId);
                return View(model);
            }

            var driver = _db.Drivers.FirstOrDefault(d => d.Id == id && d.IsDeleted == false);
            if (driver is null)
            {
                return NotFound();
            }

            driver.Name = model.Name;
            driver.Surname = model.Surname;
            driver.Number = model.Number;
            driver.Price = model.Price;
            driver.ConstructorId = model.ConstructorId;

            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("{id:int}")]
        public IActionResult Details(int id)
        {
            var driver = _db.Drivers
                .Include(d => d.Constructor)
                .Include(d => d.RaceResults.Where(rr => rr.IsDeleted == false && rr.Race != null && rr.Race.IsDeleted == false))
                .FirstOrDefault(d => d.Id == id && d.IsDeleted == false && d.Constructor != null && d.Constructor.IsDeleted == false);
            if (driver is null)
            {
                return NotFound();
            }

            return View(driver);
        }

        [HttpPost("delete/{id:int}")]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var driver = _db.Drivers.FirstOrDefault(d => d.Id == id && d.IsDeleted == false);
            if (driver is null)
            {
                return NotFound();
            }

            driver.IsDeleted = true;
            driver.DeletedAt = DateTime.UtcNow;
            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        private void PopulateConstructors(int? selectedId = null)
        {
            var items = _db.Constructors
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
                Text = "- select constructor -"
            });

            ViewBag.Constructors = new SelectList(items, "Value", "Text", selectedId?.ToString());
        }

        private IQueryable<Driver> BuildDriversQuery(string? term)
        {
            var query = _db.Drivers
                .Where(d => d.IsDeleted == false && d.Constructor != null && d.Constructor.IsDeleted == false)
                .Include(d => d.Constructor)
                .Include(d => d.RaceResults.Where(rr => rr.IsDeleted == false && rr.Race != null && rr.Race.IsDeleted == false))
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(term))
            {
                query = query.Where(d => (d.Name + " " + d.Surname).Contains(term));
            }

            return query;
        }
    }
}