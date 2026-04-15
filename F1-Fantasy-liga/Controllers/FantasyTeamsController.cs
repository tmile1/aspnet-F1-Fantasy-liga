using F1_Fantasy_liga.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace F1_Fantasy_liga.Controllers
{
    public class FantasyTeamsController : Controller
    {
        private readonly FantasyTeamMockRepository _fantasyTeamRepository;

        public FantasyTeamsController(FantasyTeamMockRepository fantasyTeamRepository)
        {
            _fantasyTeamRepository = fantasyTeamRepository;
        }

        public IActionResult Index()
        {
            var fantasyTeams = _fantasyTeamRepository.GetAll();
            return View(fantasyTeams);
        }

        public IActionResult Details(int id)
        {
            var fantasyTeam = _fantasyTeamRepository.GetById(id);
            if (fantasyTeam is null)
            {
                return NotFound();
            }

            return View(fantasyTeam);
        }
    }
}