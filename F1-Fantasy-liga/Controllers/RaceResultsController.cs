using F1_Fantasy_liga.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace F1_Fantasy_liga.Controllers
{
    public class RaceResultsController : Controller
    {
        private readonly RaceResultMockRepository _raceResultRepository;

        public RaceResultsController(RaceResultMockRepository raceResultRepository)
        {
            _raceResultRepository = raceResultRepository;
        }

        public IActionResult Index(string? raceSearch)
        {
            var raceResults = _raceResultRepository.GetAll();

            if (!string.IsNullOrWhiteSpace(raceSearch))
            {
                raceResults = raceResults
                    .Where(r => (r.Race?.Name ?? string.Empty).Contains(raceSearch, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            ViewData["RaceSearch"] = raceSearch;
            return View(raceResults);
        }

        public IActionResult Details(int id)
        {
            var raceResult = _raceResultRepository.GetById(id);
            if (raceResult is null)
            {
                return NotFound();
            }

            return View(raceResult);
        }
    }
}