using F1_Fantasy_liga.Models;
using F1_Fantasy_liga.Models.DTO;
using F1_Fantasy_liga.Models.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace F1_Fantasy_liga.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersApiController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersApiController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAll([FromQuery] string? search)
        {
            var query = _userManager.Users
                .Where(u => u.IsDeleted == false)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(u => (u.Name + " " + u.Surname).Contains(search));
            }

            var users = query
                .OrderBy(u => u.Name)
                .ThenBy(u => u.Surname)
                .ToList();

            var results = new List<UserDTO>();
            foreach (var user in users)
            {
                results.Add(await ToDTOAsync(user));
            }

            return Ok(results);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetById(string id)
        {
            var user = _userManager.Users.FirstOrDefault(u => u.Id == id && u.IsDeleted == false);
            if (user is null)
            {
                return NotFound();
            }

            return Ok(await ToDTOAsync(user));
        }

        [HttpPost]
        public async Task<ActionResult<UserDTO>> Create([FromBody] UserCreateDTO model)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
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

                return ValidationProblem(ModelState);
            }

            var roleName = model.Role.ToString();
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                ModelState.AddModelError(nameof(UserCreateDTO.Role), "Role not found.");
                return ValidationProblem(ModelState);
            }

            await _userManager.AddToRoleAsync(user, roleName);

            return CreatedAtAction(nameof(GetById), new { id = user.Id }, await ToDTOAsync(user));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UserUpdateDTO model)
        {
            var user = _userManager.Users.FirstOrDefault(u => u.Id == id && u.IsDeleted == false);
            if (user is null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
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

                return ValidationProblem(ModelState);
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

                    return ValidationProblem(ModelState);
                }
            }

            var roleName = model.Role.ToString();
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                ModelState.AddModelError(nameof(UserUpdateDTO.Role), "Role not found.");
                return ValidationProblem(ModelState);
            }

            var roles = await _userManager.GetRolesAsync(user);
            if (!roles.Contains(roleName))
            {
                await _userManager.RemoveFromRolesAsync(user, roles);
                await _userManager.AddToRoleAsync(user, roleName);
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
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

            return NoContent();
        }

        private async Task<UserDTO> ToDTOAsync(AppUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var roleName = roles.FirstOrDefault() ?? Role.User.ToString();
            Enum.TryParse(roleName, out Role parsedRole);

            return new UserDTO
            {
                Id = user.Id,
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email ?? string.Empty,
                Role = parsedRole
            };
        }
    }
}
