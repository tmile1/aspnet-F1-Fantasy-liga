using F1_Fantasy_liga.Data;
using F1_Fantasy_liga.Models;
using F1_Fantasy_liga.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace F1_Fantasy_liga.Controllers
{
    [ApiController]
    [Route("api/drivers")]
    public class DriversApiController : ControllerBase
    {
        private readonly F1DbContext _db;

        public DriversApiController(F1DbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public ActionResult<IEnumerable<DriverDTO>> GetAll([FromQuery] string? search)
        {
            var query = _db.Drivers
                .Where(d => d.IsDeleted == false && d.Constructor != null && d.Constructor.IsDeleted == false)
                .Include(d => d.Constructor)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(d => (d.Name + " " + d.Surname).Contains(search));
            }

            var drivers = query
                .OrderBy(d => d.Name)
                .ThenBy(d => d.Surname)
                .ToList()
                .Select(ToDTO)
                .ToList();

            return Ok(drivers);
        }

        [HttpGet("{id:int}")]
        public ActionResult<DriverDTO> GetById(int id)
        {
            var driver = _db.Drivers
                .Include(d => d.Constructor)
                .FirstOrDefault(d => d.Id == id && d.IsDeleted == false && d.Constructor != null && d.Constructor.IsDeleted == false);

            if (driver is null)
            {
                return NotFound();
            }

            return Ok(ToDTO(driver));
        }

        [HttpPost]
        public ActionResult<DriverDTO> Create([FromBody] DriverCreateDTO model)
        {
            ValidateConstructor(model.ConstructorId);
            ValidateNumberUnique(model.Number, null);

            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var driver = new Driver
            {
                Name = model.Name,
                Surname = model.Surname,
                Number = model.Number,
                Price = model.Price,
                ConstructorId = model.ConstructorId,
                IsDeleted = false,
                DeletedAt = null
            };

            _db.Drivers.Add(driver);
            _db.SaveChanges();

            var createdDriver = _db.Drivers
                .Include(d => d.Constructor)
                .First(d => d.Id == driver.Id);

            return CreatedAtAction(nameof(GetById), new { id = driver.Id }, ToDTO(createdDriver));
        }

        [HttpPut("{id:int}")]
        public IActionResult Update(int id, [FromBody] DriverUpdateDTO model)
        {
            var driver = _db.Drivers.FirstOrDefault(d => d.Id == id && d.IsDeleted == false);
            if (driver is null)
            {
                return NotFound();
            }

            ValidateConstructor(model.ConstructorId);
            ValidateNumberUnique(model.Number, id);

            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            driver.Name = model.Name;
            driver.Surname = model.Surname;
            driver.Number = model.Number;
            driver.Price = model.Price;
            driver.ConstructorId = model.ConstructorId;

            _db.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
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

            return NoContent();
        }

        private void ValidateConstructor(int constructorId)
        {
            if (constructorId <= 0)
            {
                ModelState.AddModelError(nameof(DriverCreateDTO.ConstructorId), "Constructor is required.");
                return;
            }

            var exists = _db.Constructors.Any(c => c.Id == constructorId && c.IsDeleted == false);
            if (!exists)
            {
                ModelState.AddModelError(nameof(DriverCreateDTO.ConstructorId), "Constructor not found.");
            }
        }

        private void ValidateNumberUnique(int number, int? currentId)
        {
            var exists = _db.Drivers.Any(d => d.IsDeleted == false
                && d.Number == number
                && (!currentId.HasValue || d.Id != currentId.Value));

            if (exists)
            {
                ModelState.AddModelError(nameof(DriverCreateDTO.Number), "Driver number is already taken.");
            }
        }

        private static DriverDTO ToDTO(Driver driver)
        {
            return new DriverDTO
            {
                Id = driver.Id,
                Name = driver.Name,
                Surname = driver.Surname,
                Number = driver.Number,
                Price = driver.Price,
                Constructor = driver.Constructor == null
                    ? null
                    : new ConstructorSummaryDTO
                    {
                        Id = driver.Constructor.Id,
                        Name = driver.Constructor.Name
                    }
            };
        }
    }
}
