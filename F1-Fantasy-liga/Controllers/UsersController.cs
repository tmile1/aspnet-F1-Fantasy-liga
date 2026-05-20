using F1_Fantasy_liga.Data;
using F1_Fantasy_liga.Models;
using F1_Fantasy_liga.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace F1_Fantasy_liga.Controllers
{
    [Route("users")]
    public class UsersController : Controller
    {
        private readonly F1DbContext _db;

        public UsersController(F1DbContext db)
        {
            _db = db;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            var users = BuildUsersQuery(null).ToList();
            return View(users);
        }

        [HttpGet("search")]
        public IActionResult Search(string? term)
        {
            var users = BuildUsersQuery(term).ToList();
            return PartialView("_UsersTableBody", users);
        }

        [HttpGet("create")]
        public IActionResult Create()
        {
            PopulateRoles();
            return View(new User());
        }

        [HttpGet("autocomplete")]
        public IActionResult Autocomplete(string? term)
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

        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Name,Surname,Email,PasswordHash,ConfirmPassword,Role")] User model)
        {
            if (model.PasswordHash != model.ConfirmPassword)
            {
                ModelState.AddModelError(nameof(F1_Fantasy_liga.Models.User.ConfirmPassword), "Passwords do not match.");
            }

            if (!ModelState.IsValid)
            {
                PopulateRoles(model.Role);
                return View(model);
            }

            model.IsDeleted = false;
            model.DeletedAt = null;

            _db.Users.Add(model);
            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("{id:int}")]
        public IActionResult Details(int id)
        {
            var user = _db.Users
                .Include(u => u.FantasyTeams.Where(ft => ft.IsDeleted == false && ft.FantasyLeague != null && ft.FantasyLeague.IsDeleted == false))
                .ThenInclude(ft => ft.FantasyLeague)
                .Include(u => u.FantasyTeams.Where(ft => ft.IsDeleted == false && ft.FantasyLeague != null && ft.FantasyLeague.IsDeleted == false))
                .ThenInclude(ft => ft.Drivers.Where(d => d.IsDeleted == false))
                .ThenInclude(d => d.RaceResults.Where(rr => rr.IsDeleted == false && rr.Race != null && rr.Race.IsDeleted == false))
                .Include(u => u.FantasyTeams.Where(ft => ft.IsDeleted == false && ft.FantasyLeague != null && ft.FantasyLeague.IsDeleted == false))
                .ThenInclude(ft => ft.Constructor)
                .ThenInclude(c => c.Drivers.Where(d => d.IsDeleted == false))
                .ThenInclude(d => d.RaceResults.Where(rr => rr.IsDeleted == false && rr.Race != null && rr.Race.IsDeleted == false))
                .FirstOrDefault(u => u.Id == id && u.IsDeleted == false);
            if (user is null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpGet("edit/{id:int}")]
        public IActionResult Edit(int id)
        {
            var user = _db.Users.FirstOrDefault(u => u.Id == id && u.IsDeleted == false);
            if (user is null)
            {
                return NotFound();
            }

            PopulateRoles(user.Role);
            return View(user);
        }

        [HttpPost("edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Name,Surname,Email,PasswordHash,ConfirmPassword,Role")] User model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            if (model.PasswordHash != model.ConfirmPassword)
            {
                ModelState.AddModelError(nameof(F1_Fantasy_liga.Models.User.ConfirmPassword), "Passwords do not match.");
            }

            if (!ModelState.IsValid)
            {
                PopulateRoles(model.Role);
                return View(model);
            }

            var user = _db.Users.FirstOrDefault(u => u.Id == id && u.IsDeleted == false);
            if (user is null)
            {
                return NotFound();
            }

            user.Name = model.Name;
            user.Surname = model.Surname;
            user.Email = model.Email;
            user.PasswordHash = model.PasswordHash;
            user.Role = model.Role;

            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost("delete/{id:int}")]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var user = _db.Users.FirstOrDefault(u => u.Id == id && u.IsDeleted == false);
            if (user is null)
            {
                return NotFound();
            }

            user.IsDeleted = true;
            user.DeletedAt = DateTime.UtcNow;
            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        private void PopulateRoles(Role? selectedRole = null)
        {
            var items = Enum.GetValues<Role>()
                .Select(role => new SelectListItem
                {
                    Value = role.ToString(),
                    Text = role.ToString()
                })
                .ToList();

            ViewBag.Roles = new SelectList(items, "Value", "Text", selectedRole?.ToString());
        }

        private IQueryable<User> BuildUsersQuery(string? term)
        {
            var query = _db.Users
                .Where(u => u.IsDeleted == false)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(term))
            {
                query = query.Where(u => (u.Name + " " + u.Surname).Contains(term));
            }

            return query;
        }
    }
}