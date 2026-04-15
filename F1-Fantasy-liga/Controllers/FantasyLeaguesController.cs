using F1_Fantasy_liga.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace F1_Fantasy_liga.Controllers
{
    public class FantasyLeaguesController : Controller
    {
        private readonly FantasyLeagueMockRepository _fantasyLeagueRepository;

        public FantasyLeaguesController(FantasyLeagueMockRepository fantasyLeagueRepository)
        {
            _fantasyLeagueRepository = fantasyLeagueRepository;
        }

        public IActionResult Index()
        {
            var fantasyLeagues = _fantasyLeagueRepository.GetAll();
            return View(fantasyLeagues);
        }

        public IActionResult Details(int id)
        {
            var fantasyLeague = _fantasyLeagueRepository.GetById(id);
            if (fantasyLeague is null)
            {
                return NotFound();
            }

            return View(fantasyLeague);
        }
    }
}