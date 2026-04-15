using F1_Fantasy_liga.Models;
using F1_Fantasy_liga.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace F1_Fantasy_liga.Controllers
{
    public class BuildTeamController : Controller
    {
        private readonly DriverMockRepository _driverRepository;

        public BuildTeamController(DriverMockRepository driverRepository)
        {
            _driverRepository = driverRepository;
        }

        public IActionResult Index()
        {
            var model = new BuildFantasyTeamViewModel
            {
                Drivers = _driverRepository.GetAll()
            };

            return View(model);
        }
    }
}