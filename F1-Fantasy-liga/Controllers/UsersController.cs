using F1_Fantasy_liga.Data;
using F1_Fantasy_liga.Models;
using F1_Fantasy_liga.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace F1_Fantasy_liga.Controllers
{
    [Route("users")]
    public class UsersController : Controller
    {
        private readonly F1DbContext _db;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController(F1DbContext db, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var users = await BuildUsersQuery(null).ToListAsync();
            var items = new List<UserListItemViewModel>();

            foreach (var user in users)
            {
                items.Add(await ToListItemAsync(user));
            }

            return View(items);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search(string? term)
        {
            var users = await BuildUsersQuery(term).ToListAsync();
            var items = new List<UserListItemViewModel>();

            foreach (var user in users)
            {
                items.Add(await ToListItemAsync(user));
            }

            return PartialView("_UsersTableBody", items);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("create")]
        public IActionResult Create()
        {
            PopulateRoles();
            return View(new UserCreateViewModel());
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("autocomplete")]
        public IActionResult Autocomplete(string? term)
        {
            var results = _userManager.Users
                .Where(u => u.IsDeleted == false)
                .Where(u => string.IsNullOrWhiteSpace(term) || (u.Name + " " + u.Surname).Contains(term))
                .OrderBy(u => u.Name)
                .Select(u => new { id = u.Id, label = u.Name + " " + u.Surname })
                .Take(20)
                .ToList();

            return Json(results);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                PopulateRoles(model.Role);
                return View(model);
            }

            var user = new AppUser
            {
                Name = model.Name,
                Surname = model.Surname,
                Email = model.Email,
                UserName = model.Email,
                IsDeleted = false,
                DeletedAt = null
            };

            var createResult = await _userManager.CreateAsync(user, model.Password);
            if (!createResult.Succeeded)
            {
                foreach (var error in createResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                PopulateRoles(model.Role);
                return View(model);
            }

            if (!await _roleManager.RoleExistsAsync(model.Role))
            {
                ModelState.AddModelError(nameof(UserCreateViewModel.Role), "Role not found.");
                PopulateRoles(model.Role);
                return View(model);
            }

            await _userManager.AddToRoleAsync(user, model.Role);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Details(string id)
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

            var roles = await _userManager.GetRolesAsync(user);
            var viewModel = new UserDetailsViewModel
            {
                User = user,
                Role = roles.FirstOrDefault() ?? "User"
            };

            return View(viewModel);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("edit/{id}")]
        public async Task<IActionResult> Edit(string id)
        {
            var user = _userManager.Users.FirstOrDefault(u => u.Id == id && u.IsDeleted == false);
            if (user is null)
            {
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(user);
            var viewModel = new UserEditViewModel
            {
                Id = user.Id,
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email,
                Role = roles.FirstOrDefault() ?? "User"
            };

            PopulateRoles(viewModel.Role);
            return View(viewModel);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, UserEditViewModel model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                PopulateRoles(model.Role);
                return View(model);
            }

            var user = _userManager.Users.FirstOrDefault(u => u.Id == id && u.IsDeleted == false);
            if (user is null)
            {
                return NotFound();
            }

            user.Name = model.Name;
            user.Surname = model.Surname;
            user.Email = model.Email;
            user.UserName = model.Email;

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                foreach (var error in updateResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                PopulateRoles(model.Role);
                return View(model);
            }

            if (!string.IsNullOrWhiteSpace(model.Password))
            {
                var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                var passwordResult = await _userManager.ResetPasswordAsync(user, resetToken, model.Password);
                if (!passwordResult.Succeeded)
                {
                    foreach (var error in passwordResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }

                    PopulateRoles(model.Role);
                    return View(model);
                }
            }

            if (!await _roleManager.RoleExistsAsync(model.Role))
            {
                ModelState.AddModelError(nameof(UserEditViewModel.Role), "Role not found.");
                PopulateRoles(model.Role);
                return View(model);
            }

            var roles = await _userManager.GetRolesAsync(user);
            if (!roles.Contains(model.Role))
            {
                await _userManager.RemoveFromRolesAsync(user, roles);
                await _userManager.AddToRoleAsync(user, model.Role);
            }

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            var user = _userManager.Users.FirstOrDefault(u => u.Id == id && u.IsDeleted == false);
            if (user is null)
            {
                return NotFound();
            }

            user.IsDeleted = true;
            user.DeletedAt = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            return RedirectToAction(nameof(Index));
        }

        private void PopulateRoles(string? selectedRole = null)
        {
            var items = _roleManager.Roles
                .OrderBy(role => role.Name)
                .Select(role => new SelectListItem
                {
                    Value = role.Name ?? string.Empty,
                    Text = role.Name ?? string.Empty
                })
                .ToList();

            ViewBag.Roles = new SelectList(items, "Value", "Text", selectedRole);
        }

        private IQueryable<AppUser> BuildUsersQuery(string? term)
        {
            var query = _userManager.Users
                .Where(u => u.IsDeleted == false)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(term))
            {
                query = query.Where(u => (u.Name + " " + u.Surname).Contains(term));
            }

            return query;
        }

        private async Task<UserListItemViewModel> ToListItemAsync(AppUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            return new UserListItemViewModel
            {
                Id = user.Id,
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email ?? string.Empty,
                Role = roles.FirstOrDefault() ?? "User"
            };
        }
    }
}