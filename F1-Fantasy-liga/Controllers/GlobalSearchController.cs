using F1_Fantasy_liga.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace F1_Fantasy_liga.Controllers
{
    [Route("search")]
    public class GlobalSearchController : Controller
    {
        private readonly F1DbContext _db;

        public GlobalSearchController(F1DbContext db)
        {
            _db = db;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index(string? term)
        {
            var normalizedTerm = term?.Trim() ?? string.Empty;
            var loweredTerm = normalizedTerm.ToLower();

            if (normalizedTerm.Length < 2)
            {
                return Json(CreateEmptyResponse());
            }

            var drivers = await _db.Drivers
                .AsNoTracking()
                .Where(driver => driver.IsDeleted == false && driver.Constructor != null && driver.Constructor.IsDeleted == false)
                .Where(driver => (driver.Name + " " + driver.Surname).ToLower().Contains(loweredTerm))
                .OrderBy(driver => driver.Name)
                .ThenBy(driver => driver.Surname)
                .Take(5)
                .Select(driver => new GlobalSearchDriverResultDto(
                    driver.Id,
                    driver.Name + " " + driver.Surname,
                    $"/drivers/{driver.Id}"))
                .ToListAsync();

            var constructors = await _db.Constructors
                .AsNoTracking()
                .Where(constructor => constructor.IsDeleted == false && constructor.Name.ToLower().Contains(loweredTerm))
                .OrderBy(constructor => constructor.Name)
                .Take(5)
                .Select(constructor => new GlobalSearchNamedResultDto(
                    constructor.Id,
                    constructor.Name,
                    $"/constructors/{constructor.Id}"))
                .ToListAsync();

            var circuits = await _db.Circuits
                .AsNoTracking()
                .Where(circuit => circuit.IsDeleted == false && (circuit.Name.ToLower().Contains(loweredTerm) || circuit.Country.ToLower().Contains(loweredTerm)))
                .OrderBy(circuit => circuit.Name)
                .Take(5)
                .Select(circuit => new GlobalSearchNamedResultDto(
                    circuit.Id,
                    circuit.Name,
                    $"/circuits/{circuit.Id}"))
                .ToListAsync();

            var races = await _db.Races
                .AsNoTracking()
                .Where(race => race.IsDeleted == false && race.Circuit != null && race.Circuit.IsDeleted == false && race.Name.ToLower().Contains(loweredTerm))
                .OrderBy(race => race.Name)
                .Take(5)
                .Select(race => new GlobalSearchNamedResultDto(
                    race.Id,
                    race.Name,
                    $"/races/{race.Id}"))
                .ToListAsync();

            var fantasyLeagues = await _db.FantasyLeagues
                .AsNoTracking()
                .Where(fantasyLeague => fantasyLeague.IsDeleted == false && fantasyLeague.Name.ToLower().Contains(loweredTerm))
                .OrderBy(fantasyLeague => fantasyLeague.Name)
                .Take(5)
                .Select(fantasyLeague => new GlobalSearchNamedResultDto(
                    fantasyLeague.Id,
                    fantasyLeague.Name,
                    $"/fantasy-leagues/{fantasyLeague.Id}"))
                .ToListAsync();

            var fantasyTeams = await _db.FantasyTeams
                .AsNoTracking()
                .Where(fantasyTeam => fantasyTeam.IsDeleted == false && fantasyTeam.User != null && fantasyTeam.User.IsDeleted == false && fantasyTeam.FantasyLeague != null && fantasyTeam.FantasyLeague.IsDeleted == false)
                .Where(fantasyTeam => fantasyTeam.Name.ToLower().Contains(loweredTerm))
                .OrderBy(fantasyTeam => fantasyTeam.Name)
                .Take(5)
                .Select(fantasyTeam => new GlobalSearchNamedResultDto(
                    fantasyTeam.Id,
                    fantasyTeam.Name,
                    $"/fantasy-teams/{fantasyTeam.Id}"))
                .ToListAsync();

            return Json(new GlobalSearchResponseDto
            {
                Drivers = drivers,
                Constructors = constructors,
                Circuits = circuits,
                Races = races,
                FantasyLeagues = fantasyLeagues,
                FantasyTeams = fantasyTeams
            });
        }

        private static GlobalSearchResponseDto CreateEmptyResponse()
        {
            return new GlobalSearchResponseDto();
        }
    }

    public sealed class GlobalSearchResponseDto
    {
        [JsonPropertyName("Drivers")]
        public List<GlobalSearchDriverResultDto> Drivers { get; set; } = new();

        [JsonPropertyName("Constructors")]
        public List<GlobalSearchNamedResultDto> Constructors { get; set; } = new();

        [JsonPropertyName("Circuits")]
        public List<GlobalSearchNamedResultDto> Circuits { get; set; } = new();

        [JsonPropertyName("Races")]
        public List<GlobalSearchNamedResultDto> Races { get; set; } = new();

        [JsonPropertyName("FantasyLeagues")]
        public List<GlobalSearchNamedResultDto> FantasyLeagues { get; set; } = new();

        [JsonPropertyName("FantasyTeams")]
        public List<GlobalSearchNamedResultDto> FantasyTeams { get; set; } = new();
    }

    public sealed record GlobalSearchDriverResultDto(
        [property: JsonPropertyName("id")] int Id,
        [property: JsonPropertyName("fullName")] string FullName,
        [property: JsonPropertyName("url")] string Url)
    ;

    public sealed record GlobalSearchNamedResultDto(
        [property: JsonPropertyName("id")] int Id,
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("url")] string Url);
}