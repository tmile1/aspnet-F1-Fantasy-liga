using F1_Fantasy_liga.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace F1_Fantasy_liga.Controllers
{
    public class RacesController : Controller
    {
        private readonly RaceMockRepository _raceRepository;

        public RacesController(RaceMockRepository raceRepository)
        {
            _raceRepository = raceRepository;
        }

        public IActionResult Index()
        {
            var races = _raceRepository.GetAll();
            return View(races);
        }

        public IActionResult Details(int id)
        {
            var race = _raceRepository.GetById(id);
            if (race is null)
            {
                return NotFound();
            }

            return View(race);
        }
    }
}