using F1_Fantasy_liga.Data;
using F1_Fantasy_liga.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace F1_Fantasy_liga.Controllers
{
    [Route("fantasy-teams")]
    public class FantasyTeamsController : Controller
    {
        private readonly F1DbContext _db;

        public FantasyTeamsController(F1DbContext db)
        {
            _db = db;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            var fantasyTeams = BuildFantasyTeamsQuery(null).ToList();

            foreach (var team in fantasyTeams)
            {
                if (team.Constructor != null && team.Constructor.IsDeleted)
                {
                    team.Constructor = null;
                }
            }

            return View(fantasyTeams);
        }

        [HttpGet("search")]
        public IActionResult Search(string? term)
        {
            var fantasyTeams = BuildFantasyTeamsQuery(term).ToList();

            foreach (var team in fantasyTeams)
            {
                if (team.Constructor != null && team.Constructor.IsDeleted)
                {
                    team.Constructor = null;
                }
            }

            return PartialView("_FantasyTeamsCards", fantasyTeams);
        }

        [HttpGet("create")]
        public IActionResult Create()
        {
            PopulateUsers();
            PopulateConstructors();
            PopulateLeagues();
            return View(new FantasyTeam());
        }

        [HttpGet("autocomplete/users")]
        public IActionResult AutocompleteUsers(string? term)
        {
            var results = _db.Users
                .Where(u => u.IsDeleted == false)
                .Where(u => string.IsNullOrWhiteSpace(term) || (u.Name + " " + u.Surname).Contains(term))
                .OrderBy(u => u.Name)
                .Select(u => new { id = u.Id, label = u.Name + " " + u.Surname })
                .Take(20)
                .ToList();

            return Json(results);
        }

        [HttpGet("autocomplete/constructors")]
        public IActionResult AutocompleteConstructors(string? term)
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

        [HttpGet("autocomplete/leagues")]
        public IActionResult AutocompleteLeagues(string? term)
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

        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Name,Budget,UserId,ConstructorId,FantasyLeagueId")] FantasyTeam model)
        {
            ValidateFantasyTeamSelections(model);

            if (!ModelState.IsValid)
            {
                if (model.UserId > 0)
                {
                    ViewBag.UserName = _db.Users
                        .Where(u => u.Id == model.UserId && u.IsDeleted == false)
                        .Select(u => u.Name + " " + u.Surname)
                        .FirstOrDefault();
                }

                if (model.ConstructorId > 0)
                {
                    ViewBag.ConstructorName = _db.Constructors
                        .Where(c => c.Id == model.ConstructorId && c.IsDeleted == false)
                        .Select(c => c.Name)
                        .FirstOrDefault();
                }

                if (model.FantasyLeagueId > 0)
                {
                    ViewBag.LeagueName = _db.FantasyLeagues
                        .Where(fl => fl.Id == model.FantasyLeagueId && fl.IsDeleted == false)
                        .Select(fl => fl.Name)
                        .FirstOrDefault();
                }

                PopulateUsers(model.UserId);
                PopulateConstructors(model.ConstructorId);
                PopulateLeagues(model.FantasyLeagueId);
                return View(model);
            }

            model.IsDeleted = false;
            model.DeletedAt = null;

            _db.FantasyTeams.Add(model);
            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("{id:int}")]
        public IActionResult Details(int id)
        {
            var fantasyTeam = _db.FantasyTeams
                .Include(ft => ft.User)
                .Include(ft => ft.FantasyLeague)
                .Include(ft => ft.Drivers.Where(d => d.IsDeleted == false && d.Constructor != null && d.Constructor.IsDeleted == false))
                .ThenInclude(d => d.Constructor)
                .Include(ft => ft.Drivers.Where(d => d.IsDeleted == false && d.Constructor != null && d.Constructor.IsDeleted == false))
                .ThenInclude(d => d.RaceResults.Where(rr => rr.IsDeleted == false && rr.Race != null && rr.Race.IsDeleted == false))
                .Include(ft => ft.Constructor)
                .ThenInclude(c => c.Drivers.Where(d => d.IsDeleted == false))
                .ThenInclude(d => d.RaceResults.Where(rr => rr.IsDeleted == false && rr.Race != null && rr.Race.IsDeleted == false))
                .FirstOrDefault(ft => ft.Id == id && ft.IsDeleted == false && ft.User != null && ft.User.IsDeleted == false && ft.FantasyLeague != null && ft.FantasyLeague.IsDeleted == false);
            if (fantasyTeam is null)
            {
                return NotFound();
            }

            if (fantasyTeam.Constructor != null && fantasyTeam.Constructor.IsDeleted)
            {
                fantasyTeam.Constructor = null;
            }

            return View(fantasyTeam);
        }

        [HttpGet("edit/{id:int}")]
        public IActionResult Edit(int id)
        {
            var fantasyTeam = _db.FantasyTeams
                .Include(ft => ft.User)
                .Include(ft => ft.FantasyLeague)
                .FirstOrDefault(ft => ft.Id == id && ft.IsDeleted == false && ft.User != null && ft.User.IsDeleted == false && ft.FantasyLeague != null && ft.FantasyLeague.IsDeleted == false);
            if (fantasyTeam is null)
            {
                return NotFound();
            }

            PopulateUsers(fantasyTeam.UserId);
            PopulateConstructors(fantasyTeam.ConstructorId);
            PopulateLeagues(fantasyTeam.FantasyLeagueId);
            return View(fantasyTeam);
        }

        [HttpPost("edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Name,Budget,UserId,ConstructorId,FantasyLeagueId")] FantasyTeam model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            ValidateFantasyTeamSelections(model);

            if (!ModelState.IsValid)
            {
                if (model.UserId > 0)
                {
                    ViewBag.UserName = _db.Users
                        .Where(u => u.Id == model.UserId && u.IsDeleted == false)
                        .Select(u => u.Name + " " + u.Surname)
                        .FirstOrDefault();
                }

                if (model.ConstructorId > 0)
                {
                    ViewBag.ConstructorName = _db.Constructors
                        .Where(c => c.Id == model.ConstructorId && c.IsDeleted == false)
                        .Select(c => c.Name)
                        .FirstOrDefault();
                }

                if (model.FantasyLeagueId > 0)
                {
                    ViewBag.LeagueName = _db.FantasyLeagues
                        .Where(fl => fl.Id == model.FantasyLeagueId && fl.IsDeleted == false)
                        .Select(fl => fl.Name)
                        .FirstOrDefault();
                }

                PopulateUsers(model.UserId);
                PopulateConstructors(model.ConstructorId);
                PopulateLeagues(model.FantasyLeagueId);
                return View(model);
            }

            var fantasyTeam = _db.FantasyTeams
                .Include(ft => ft.User)
                .Include(ft => ft.FantasyLeague)
                .FirstOrDefault(ft => ft.Id == id && ft.IsDeleted == false && ft.User != null && ft.User.IsDeleted == false && ft.FantasyLeague != null && ft.FantasyLeague.IsDeleted == false);
            if (fantasyTeam is null)
            {
                return NotFound();
            }

            fantasyTeam.Name = model.Name;
            fantasyTeam.Budget = model.Budget;
            fantasyTeam.UserId = model.UserId;
            fantasyTeam.ConstructorId = model.ConstructorId;
            fantasyTeam.FantasyLeagueId = model.FantasyLeagueId;

            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost("delete/{id:int}")]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var fantasyTeam = _db.FantasyTeams.FirstOrDefault(ft => ft.Id == id && ft.IsDeleted == false);
            if (fantasyTeam is null)
            {
                return NotFound();
            }

            fantasyTeam.IsDeleted = true;
            fantasyTeam.DeletedAt = DateTime.UtcNow;
            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("manage-drivers/{id:int}")]
        public IActionResult ManageDrivers(int id)
        {
            var fantasyTeam = _db.FantasyTeams
                .Include(ft => ft.Drivers)
                .ThenInclude(d => d.Constructor)
                .FirstOrDefault(ft => ft.Id == id && ft.IsDeleted == false && ft.User != null && ft.User.IsDeleted == false && ft.FantasyLeague != null && ft.FantasyLeague.IsDeleted == false);
            if (fantasyTeam is null)
            {
                return NotFound();
            }

            var selectedDrivers = fantasyTeam.Drivers
                .Where(d => d.IsDeleted == false && d.Constructor != null && d.Constructor.IsDeleted == false)
                .ToList();
            var selectedDriverIds = selectedDrivers.Select(d => d.Id).ToHashSet();

            var availableDrivers = _db.Drivers
                .Where(d => d.IsDeleted == false && d.Constructor != null && d.Constructor.IsDeleted == false && !selectedDriverIds.Contains(d.Id))
                .Include(d => d.Constructor)
                .ToList();

            var model = new ManageFantasyTeamDriversViewModel
            {
                TeamId = fantasyTeam.Id,
                TeamName = fantasyTeam.Name,
                BudgetLimit = fantasyTeam.Budget,
                SelectedDrivers = selectedDrivers,
                AvailableDrivers = availableDrivers
            };

            return View(model);
        }

        [HttpPost("manage-drivers/{id:int}")]
        [ValidateAntiForgeryToken]
        public IActionResult ManageDrivers(int id, string? driverIds)
        {
            var fantasyTeam = _db.FantasyTeams
                .Include(ft => ft.Drivers)
                .FirstOrDefault(ft => ft.Id == id && ft.IsDeleted == false && ft.User != null && ft.User.IsDeleted == false && ft.FantasyLeague != null && ft.FantasyLeague.IsDeleted == false);
            if (fantasyTeam is null)
            {
                return NotFound();
            }

            var parsedIds = (driverIds ?? string.Empty)
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(value => int.TryParse(value, out var parsed) ? parsed : 0)
                .Where(value => value > 0)
                .Distinct()
                .ToList();

            var selectedDrivers = _db.Drivers
                .Where(d => parsedIds.Contains(d.Id) && d.IsDeleted == false && d.Constructor != null && d.Constructor.IsDeleted == false)
                .Include(d => d.Constructor)
                .ToList();

            var totalCost = selectedDrivers.Sum(d => d.Price);
            if (totalCost > fantasyTeam.Budget)
            {
                ModelState.AddModelError(string.Empty, "Selected drivers exceed the team budget.");
            }

            if (!ModelState.IsValid)
            {
                var selectedDriverIds = selectedDrivers.Select(d => d.Id).ToHashSet();
                var availableDrivers = _db.Drivers
                    .Where(d => d.IsDeleted == false && d.Constructor != null && d.Constructor.IsDeleted == false && !selectedDriverIds.Contains(d.Id))
                    .Include(d => d.Constructor)
                    .ToList();

                var model = new ManageFantasyTeamDriversViewModel
                {
                    TeamId = fantasyTeam.Id,
                    TeamName = fantasyTeam.Name,
                    BudgetLimit = fantasyTeam.Budget,
                    SelectedDrivers = selectedDrivers,
                    AvailableDrivers = availableDrivers
                };

                return View(model);
            }

            fantasyTeam.Drivers = selectedDrivers;
            _db.SaveChanges();

            return RedirectToAction(nameof(Edit), new { id = fantasyTeam.Id });
        }

        private void ValidateFantasyTeamSelections(FantasyTeam model)
        {
            if (model.UserId <= 0)
            {
                ModelState.AddModelError(nameof(FantasyTeam.UserId), "User is required.");
            }

            if (model.ConstructorId <= 0)
            {
                ModelState.AddModelError(nameof(FantasyTeam.ConstructorId), "Constructor is required.");
            }

            if (model.FantasyLeagueId <= 0)
            {
                ModelState.AddModelError(nameof(FantasyTeam.FantasyLeagueId), "Fantasy league is required.");
            }
        }

        private void PopulateUsers(int? selectedId = null)
        {
            var items = _db.Users
                .Where(u => u.IsDeleted == false)
                .OrderBy(u => u.Name)
                .Select(u => new SelectListItem
                {
                    Value = u.Id.ToString(),
                    Text = u.Name + " " + u.Surname
                })
                .ToList();

            items.Insert(0, new SelectListItem
            {
                Value = string.Empty,
                Text = "- select user -"
            });

            ViewBag.Users = new SelectList(items, "Value", "Text", selectedId?.ToString());
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

        private void PopulateLeagues(int? selectedId = null)
        {
            var items = _db.FantasyLeagues
                .Where(fl => fl.IsDeleted == false)
                .OrderBy(fl => fl.Name)
                .Select(fl => new SelectListItem
                {
                    Value = fl.Id.ToString(),
                    Text = fl.Name
                })
                .ToList();

            items.Insert(0, new SelectListItem
            {
                Value = string.Empty,
                Text = "- select league -"
            });

            ViewBag.Leagues = new SelectList(items, "Value", "Text", selectedId?.ToString());
        }

        private IQueryable<FantasyTeam> BuildFantasyTeamsQuery(string? term)
        {
            var query = _db.FantasyTeams
                .Where(ft => ft.IsDeleted == false && ft.User != null && ft.User.IsDeleted == false && ft.FantasyLeague != null && ft.FantasyLeague.IsDeleted == false)
                .Include(ft => ft.User)
                .Include(ft => ft.FantasyLeague)
                .Include(ft => ft.Drivers.Where(d => d.IsDeleted == false && d.Constructor != null && d.Constructor.IsDeleted == false))
                .ThenInclude(d => d.RaceResults.Where(rr => rr.IsDeleted == false && rr.Race != null && rr.Race.IsDeleted == false))
                .Include(ft => ft.Constructor)
                .ThenInclude(c => c.Drivers.Where(d => d.IsDeleted == false))
                .ThenInclude(d => d.RaceResults.Where(rr => rr.IsDeleted == false && rr.Race != null && rr.Race.IsDeleted == false))
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(term))
            {
                query = query.Where(ft => ft.Name.Contains(term));
            }

            return query;
        }
    }
}