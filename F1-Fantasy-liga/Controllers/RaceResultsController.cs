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

        public IActionResult Index()
        {
            var raceResults = _raceResultRepository.GetAll();
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