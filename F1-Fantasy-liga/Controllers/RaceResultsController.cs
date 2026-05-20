using F1_Fantasy_liga.Data;
using F1_Fantasy_liga.Models;
using F1_Fantasy_liga.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

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
            var results = BuildRaceResultsQuery(raceSearch).ToList();
            ViewData["RaceSearch"] = raceSearch;
            return View(results);
        }

        [HttpGet("search")]
        public IActionResult Search(string? term)
        {
            var results = BuildRaceResultsQuery(term).ToList();
            return PartialView("_RaceResultsTableBody", results);
        }

        [HttpGet("create")]
        public IActionResult Create()
        {
            PopulateDrivers();
            PopulateRaces();
            PopulateDriverStatuses();
            return View(new RaceResult());
        }

        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("FinishedPosition,DriverId,RaceId,DriverStatus")] RaceResult model)
        {
            ValidateRaceResultSelections(model, null);

            model.ScoredPoints = CalculateScoredPoints(model.DriverStatus, model.FinishedPosition);

            if (!ModelState.IsValid)
            {
                if (model.DriverId > 0)
                {
                    ViewBag.DriverName = _db.Drivers
                        .Where(d => d.Id == model.DriverId && d.IsDeleted == false)
                        .Select(d => d.Name + " " + d.Surname)
                        .FirstOrDefault();
                }

                if (model.RaceId > 0)
                {
                    ViewBag.RaceName = _db.Races
                        .Where(r => r.Id == model.RaceId && r.IsDeleted == false)
                        .Select(r => r.Name)
                        .FirstOrDefault();
                }

                PopulateDrivers(model.DriverId);
                PopulateRaces(model.RaceId);
                PopulateDriverStatuses(model.DriverStatus);
                return View(model);
            }

            model.IsDeleted = false;
            model.DeletedAt = null;

            _db.RaceResults.Add(model);
            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("{id:int}")]
        public IActionResult Details(int id)
        {
            var raceResult = _db.RaceResults
                .Include(r => r.Race)
                .Include(r => r.Driver)
                .ThenInclude(d => d.Constructor)
                .FirstOrDefault(r => r.Id == id && r.IsDeleted == false && r.Race != null && r.Race.IsDeleted == false && r.Driver != null && r.Driver.IsDeleted == false);
            if (raceResult is null)
            {
                return NotFound();
            }

            return View(raceResult);
        }

        [HttpGet("edit/{id:int}")]
        public IActionResult Edit(int id)
        {
            var raceResult = _db.RaceResults
                .Include(r => r.Race)
                .Include(r => r.Driver)
                .FirstOrDefault(r => r.Id == id && r.IsDeleted == false && r.Race != null && r.Race.IsDeleted == false && r.Driver != null && r.Driver.IsDeleted == false);
            if (raceResult is null)
            {
                return NotFound();
            }

            PopulateDrivers(raceResult.DriverId);
            PopulateRaces(raceResult.RaceId);
            PopulateDriverStatuses(raceResult.DriverStatus);
            return View(raceResult);
        }

        [HttpPost("edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,FinishedPosition,DriverId,RaceId,DriverStatus")] RaceResult model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            ValidateRaceResultSelections(model, id);

            model.ScoredPoints = CalculateScoredPoints(model.DriverStatus, model.FinishedPosition);

            if (!ModelState.IsValid)
            {
                if (model.DriverId > 0)
                {
                    ViewBag.DriverName = _db.Drivers
                        .Where(d => d.Id == model.DriverId && d.IsDeleted == false)
                        .Select(d => d.Name + " " + d.Surname)
                        .FirstOrDefault();
                }

                if (model.RaceId > 0)
                {
                    ViewBag.RaceName = _db.Races
                        .Where(r => r.Id == model.RaceId && r.IsDeleted == false)
                        .Select(r => r.Name)
                        .FirstOrDefault();
                }

                PopulateDrivers(model.DriverId);
                PopulateRaces(model.RaceId);
                PopulateDriverStatuses(model.DriverStatus);
                return View(model);
            }

            var raceResult = _db.RaceResults
                .Include(r => r.Race)
                .Include(r => r.Driver)
                .FirstOrDefault(r => r.Id == id && r.IsDeleted == false && r.Race != null && r.Race.IsDeleted == false && r.Driver != null && r.Driver.IsDeleted == false);
            if (raceResult is null)
            {
                return NotFound();
            }

            raceResult.FinishedPosition = model.FinishedPosition;
            raceResult.ScoredPoints = model.ScoredPoints;
            raceResult.DriverId = model.DriverId;
            raceResult.RaceId = model.RaceId;
            raceResult.DriverStatus = model.DriverStatus;

            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost("delete/{id:int}")]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var raceResult = _db.RaceResults.FirstOrDefault(r => r.Id == id && r.IsDeleted == false);
            if (raceResult is null)
            {
                return NotFound();
            }

            raceResult.IsDeleted = true;
            raceResult.DeletedAt = DateTime.UtcNow;
            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        private void ValidateRaceResultSelections(RaceResult model, int? currentId)
        {
            if (model.DriverId <= 0)
            {
                ModelState.AddModelError(nameof(RaceResult.DriverId), "Driver is required.");
            }
            else
            {
                var driverExists = _db.Drivers.Any(d => d.Id == model.DriverId && d.IsDeleted == false && d.Constructor != null && d.Constructor.IsDeleted == false);
                if (!driverExists)
                {
                    ModelState.AddModelError(nameof(RaceResult.DriverId), "Selected driver is not available.");
                }
            }

            if (model.RaceId <= 0)
            {
                ModelState.AddModelError(nameof(RaceResult.RaceId), "Race is required.");
            }
            else
            {
                var raceExists = _db.Races.Any(r => r.Id == model.RaceId && r.IsDeleted == false && r.Circuit != null && r.Circuit.IsDeleted == false);
                if (!raceExists)
                {
                    ModelState.AddModelError(nameof(RaceResult.RaceId), "Selected race is not available.");
                }
            }
        }

        private void PopulateDrivers(int? selectedId = null)
        {
            var items = _db.Drivers
                .Where(d => d.IsDeleted == false && d.Constructor != null && d.Constructor.IsDeleted == false)
                .OrderBy(d => d.Name)
                .Select(d => new SelectListItem
                {
                    Value = d.Id.ToString(),
                    Text = d.Name + " " + d.Surname
                })
                .ToList();

            items.Insert(0, new SelectListItem
            {
                Value = string.Empty,
                Text = "- select driver -"
            });

            ViewBag.Drivers = new SelectList(items, "Value", "Text", selectedId?.ToString());
        }

        private void PopulateRaces(int? selectedId = null)
        {
            var items = _db.Races
                .Where(r => r.IsDeleted == false && r.Circuit != null && r.Circuit.IsDeleted == false)
                .OrderBy(r => r.Name)
                .Select(r => new SelectListItem
                {
                    Value = r.Id.ToString(),
                    Text = r.Name
                })
                .ToList();

            items.Insert(0, new SelectListItem
            {
                Value = string.Empty,
                Text = "- select race -"
            });

            ViewBag.Races = new SelectList(items, "Value", "Text", selectedId?.ToString());
        }

        private void PopulateDriverStatuses(DriverStatus? selectedStatus = null)
        {
            var statuses = Enum.GetValues<DriverStatus>()
                .Select(status => new SelectListItem
                {
                    Value = status.ToString(),
                    Text = status.ToString()
                })
                .ToList();

            ViewBag.DriverStatuses = new SelectList(statuses, "Value", "Text", selectedStatus?.ToString());
        }

        private IQueryable<RaceResult> BuildRaceResultsQuery(string? term)
        {
            var query = _db.RaceResults
                .Where(r => r.IsDeleted == false && r.Race != null && r.Race.IsDeleted == false && r.Driver != null && r.Driver.IsDeleted == false)
                .Include(r => r.Race)
                .Include(r => r.Driver)
                .ThenInclude(d => d.Constructor)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(term))
            {
                query = query.Where(r => r.Race != null && r.Race.Name.Contains(term));
            }

            return query;
        }

        private int CalculateScoredPoints(DriverStatus status, int finishedPosition)
        {
            if (status != DriverStatus.Finished)
            {
                return 0;
            }

            return finishedPosition switch
            {
                1 => (int)RaceResultPoints.First,
                2 => (int)RaceResultPoints.Second,
                3 => (int)RaceResultPoints.Third,
                4 => (int)RaceResultPoints.Fourth,
                5 => (int)RaceResultPoints.Fifth,
                6 => (int)RaceResultPoints.Sixth,
                7 => (int)RaceResultPoints.Seventh,
                8 => (int)RaceResultPoints.Eighth,
                9 => (int)RaceResultPoints.Ninth,
                10 => (int)RaceResultPoints.Tenth,
                _ => (int)RaceResultPoints.OutOfPoints
            };
        }
    }
}