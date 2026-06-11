using F1_Fantasy_liga.Data;
using F1_Fantasy_liga.Models;
using F1_Fantasy_liga.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace F1_Fantasy_liga.Controllers
{
    [Route("fantasy-leagues")]
    public class FantasyLeaguesController : Controller
    {
        private readonly F1DbContext _db;

        public FantasyLeaguesController(F1DbContext db)
        {
            _db = db;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            var fantasyLeagues = BuildFantasyLeaguesQuery(null).ToList();
            return View(fantasyLeagues);
        }

        [HttpGet("search")]
        public IActionResult Search(string? term)
        {
            var fantasyLeagues = BuildFantasyLeaguesQuery(term).ToList();
            return PartialView("_FantasyLeaguesCards", fantasyLeagues);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("create")]
        public IActionResult Create()
        {
            PopulateLeagueTypes();
            return View(new FantasyLeague());
        }

        [HttpGet("autocomplete")]
        public IActionResult Autocomplete(string? term)
        {
            var results = _db.FantasyLeagues
                .Where(fl => fl.IsDeleted == false)
                .Where(fl => string.IsNullOrWhiteSpace(term) || fl.Name.Contains(term))
                .OrderBy(fl => fl.Name)
                .Select(fl => new { id = fl.Id, label = fl.Name })
                .Take(20)
                .ToList();

            return Json(results);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Name,Description,StartDate,EndDate,LeagueType")] FantasyLeague model)
        {
            if (model.EndDate < model.StartDate)
            {
                ModelState.AddModelError(nameof(FantasyLeague.EndDate), "End date cannot be before start date.");
            }

            if (!ModelState.IsValid)
            {
                PopulateLeagueTypes(model.LeagueType);
                return View(model);
            }

            model.IsDeleted = false;
            model.DeletedAt = null;

            _db.FantasyLeagues.Add(model);
            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("{id:int}")]
        public IActionResult Details(int id)
        {
            var fantasyLeague = _db.FantasyLeagues
                .Include(fl => fl.FantasyTeams.Where(ft => ft.IsDeleted == false && ft.User != null && ft.User.IsDeleted == false))
                .ThenInclude(ft => ft.User)
                .Include(fl => fl.FantasyTeams.Where(ft => ft.IsDeleted == false && ft.User != null && ft.User.IsDeleted == false))
                .ThenInclude(ft => ft.Drivers.Where(d => d.IsDeleted == false))
                .ThenInclude(d => d.RaceResults.Where(rr => rr.IsDeleted == false && rr.Race != null && rr.Race.IsDeleted == false))
                .Include(fl => fl.FantasyTeams.Where(ft => ft.IsDeleted == false && ft.User != null && ft.User.IsDeleted == false))
                .ThenInclude(ft => ft.Constructor)
                .ThenInclude(c => c.Drivers.Where(d => d.IsDeleted == false))
                .ThenInclude(d => d.RaceResults.Where(rr => rr.IsDeleted == false && rr.Race != null && rr.Race.IsDeleted == false))
                .FirstOrDefault(fl => fl.Id == id && fl.IsDeleted == false);
            if (fantasyLeague is null)
            {
                return NotFound();
            }

            return View(fantasyLeague);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("edit/{id:int}")]
        public IActionResult Edit(int id)
        {
            var fantasyLeague = _db.FantasyLeagues.FirstOrDefault(fl => fl.Id == id && fl.IsDeleted == false);
            if (fantasyLeague is null)
            {
                return NotFound();
            }

            PopulateLeagueTypes(fantasyLeague.LeagueType);
            return View(fantasyLeague);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Name,Description,StartDate,EndDate,LeagueType")] FantasyLeague model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            if (model.EndDate < model.StartDate)
            {
                ModelState.AddModelError(nameof(FantasyLeague.EndDate), "End date cannot be before start date.");
            }

            if (!ModelState.IsValid)
            {
                PopulateLeagueTypes(model.LeagueType);
                return View(model);
            }

            var fantasyLeague = _db.FantasyLeagues.FirstOrDefault(fl => fl.Id == id && fl.IsDeleted == false);
            if (fantasyLeague is null)
            {
                return NotFound();
            }

            fantasyLeague.Name = model.Name;
            fantasyLeague.Description = model.Description;
            fantasyLeague.StartDate = model.StartDate;
            fantasyLeague.EndDate = model.EndDate;
            fantasyLeague.LeagueType = model.LeagueType;

            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("delete/{id:int}")]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var fantasyLeague = _db.FantasyLeagues.FirstOrDefault(fl => fl.Id == id && fl.IsDeleted == false);
            if (fantasyLeague is null)
            {
                return NotFound();
            }

            fantasyLeague.IsDeleted = true;
            fantasyLeague.DeletedAt = DateTime.UtcNow;
            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        private void PopulateLeagueTypes(LeagueType? selectedType = null)
        {
            var items = Enum.GetValues<LeagueType>()
                .Select(type => new SelectListItem
                {
                    Value = type.ToString(),
                    Text = type.ToString()
                })
                .ToList();

            ViewBag.LeagueTypes = new SelectList(items, "Value", "Text", selectedType?.ToString());
        }

        private IQueryable<FantasyLeague> BuildFantasyLeaguesQuery(string? term)
        {
            var query = _db.FantasyLeagues
                .Where(fl => fl.IsDeleted == false)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(term))
            {
                query = query.Where(fl => fl.Name.Contains(term));
            }

            return query;
        }
    }
}