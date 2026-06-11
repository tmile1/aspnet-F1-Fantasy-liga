using F1_Fantasy_liga.Data;
using F1_Fantasy_liga.Models;
using F1_Fantasy_liga.Models.DTO;
using F1_Fantasy_liga.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace F1_Fantasy_liga.Controllers
{
    [ApiController]
    [Route("api/race-results")]
    public class RaceResultsApiController : ControllerBase
    {
        private readonly F1DbContext _db;

        public RaceResultsApiController(F1DbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public ActionResult<IEnumerable<RaceResultDTO>> GetAll([FromQuery] string? search)
        {
            var query = _db.RaceResults
                .Include(rr => rr.Race)
                .ThenInclude(r => r.Circuit)
                .Include(rr => rr.Driver)
                .ThenInclude(d => d.Constructor)
                .Where(rr => rr.IsDeleted == false
                    && rr.Race != null
                    && rr.Race.IsDeleted == false
                    && rr.Race.Circuit != null
                    && rr.Race.Circuit.IsDeleted == false
                    && rr.Driver != null
                    && rr.Driver.IsDeleted == false
                    && rr.Driver.Constructor != null
                    && rr.Driver.Constructor.IsDeleted == false)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(rr => rr.Race != null && rr.Race.Name.Contains(search));
            }

            var results = query
                .OrderBy(rr => rr.RaceId)
                .ThenBy(rr => rr.FinishedPosition)
                .ToList()
                .Select(ToDTO)
                .ToList();

            return Ok(results);
        }

        [HttpGet("{id:int}")]
        public ActionResult<RaceResultDTO> GetById(int id)
        {
            var raceResult = _db.RaceResults
                .Include(rr => rr.Race)
                .ThenInclude(r => r.Circuit)
                .Include(rr => rr.Driver)
                .ThenInclude(d => d.Constructor)
                .FirstOrDefault(rr => rr.Id == id
                    && rr.IsDeleted == false
                    && rr.Race != null
                    && rr.Race.IsDeleted == false
                    && rr.Race.Circuit != null
                    && rr.Race.Circuit.IsDeleted == false
                    && rr.Driver != null
                    && rr.Driver.IsDeleted == false
                    && rr.Driver.Constructor != null
                    && rr.Driver.Constructor.IsDeleted == false);

            if (raceResult is null)
            {
                return NotFound();
            }

            return Ok(ToDTO(raceResult));
        }

        [HttpPost]
        public ActionResult<RaceResultDTO> Create([FromBody] RaceResultCreateDTO model)
        {
            ValidateDriver(model.DriverId);
            ValidateRace(model.RaceId);

            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var raceResult = new RaceResult
            {
                FinishedPosition = model.FinishedPosition,
                ScoredPoints = CalculateScoredPoints(model.DriverStatus, model.FinishedPosition),
                DriverId = model.DriverId,
                RaceId = model.RaceId,
                DriverStatus = model.DriverStatus,
                IsDeleted = false,
                DeletedAt = null
            };

            _db.RaceResults.Add(raceResult);
            _db.SaveChanges();

            var createdResult = _db.RaceResults
                .Include(rr => rr.Race)
                .ThenInclude(r => r.Circuit)
                .Include(rr => rr.Driver)
                .ThenInclude(d => d.Constructor)
                .First(rr => rr.Id == raceResult.Id);

            return CreatedAtAction(nameof(GetById), new { id = raceResult.Id }, ToDTO(createdResult));
        }

        [HttpPut("{id:int}")]
        public IActionResult Update(int id, [FromBody] RaceResultUpdateDTO model)
        {
            var raceResult = _db.RaceResults.FirstOrDefault(rr => rr.Id == id && rr.IsDeleted == false);
            if (raceResult is null)
            {
                return NotFound();
            }

            ValidateDriver(model.DriverId);
            ValidateRace(model.RaceId);

            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            raceResult.FinishedPosition = model.FinishedPosition;
            raceResult.ScoredPoints = CalculateScoredPoints(model.DriverStatus, model.FinishedPosition);
            raceResult.DriverId = model.DriverId;
            raceResult.RaceId = model.RaceId;
            raceResult.DriverStatus = model.DriverStatus;

            _db.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var raceResult = _db.RaceResults.FirstOrDefault(rr => rr.Id == id && rr.IsDeleted == false);
            if (raceResult is null)
            {
                return NotFound();
            }

            raceResult.IsDeleted = true;
            raceResult.DeletedAt = DateTime.UtcNow;
            _db.SaveChanges();

            return NoContent();
        }

        private void ValidateDriver(int driverId)
        {
            if (driverId <= 0)
            {
                ModelState.AddModelError(nameof(RaceResultCreateDTO.DriverId), "Driver is required.");
                return;
            }

            var exists = _db.Drivers.Any(d => d.Id == driverId
                && d.IsDeleted == false
                && d.Constructor != null
                && d.Constructor.IsDeleted == false);

            if (!exists)
            {
                ModelState.AddModelError(nameof(RaceResultCreateDTO.DriverId), "Selected driver is not available.");
            }
        }

        private void ValidateRace(int raceId)
        {
            if (raceId <= 0)
            {
                ModelState.AddModelError(nameof(RaceResultCreateDTO.RaceId), "Race is required.");
                return;
            }

            var exists = _db.Races.Any(r => r.Id == raceId
                && r.IsDeleted == false
                && r.Circuit != null
                && r.Circuit.IsDeleted == false);

            if (!exists)
            {
                ModelState.AddModelError(nameof(RaceResultCreateDTO.RaceId), "Selected race is not available.");
            }
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

        private static RaceResultDTO ToDTO(RaceResult raceResult)
        {
            return new RaceResultDTO
            {
                Id = raceResult.Id,
                FinishedPosition = raceResult.FinishedPosition,
                ScoredPoints = raceResult.ScoredPoints,
                DriverStatus = raceResult.DriverStatus,
                Driver = raceResult.Driver == null
                    ? null
                    : new DriverSummaryDTO
                    {
                        Id = raceResult.Driver.Id,
                        Name = raceResult.Driver.Name,
                        Surname = raceResult.Driver.Surname
                    },
                Race = raceResult.Race == null
                    ? null
                    : new RaceSummaryDTO
                    {
                        Id = raceResult.Race.Id,
                        Name = raceResult.Race.Name
                    }
            };
        }
    }
}
