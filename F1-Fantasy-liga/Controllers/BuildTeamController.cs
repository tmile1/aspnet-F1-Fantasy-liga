using F1_Fantasy_liga.Models;
using F1_Fantasy_liga.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace F1_Fantasy_liga.Controllers
{
    public class BuildTeamController : Controller
    {
        private readonly DriverMockRepository _driverRepository;
        private readonly ConstructorMockRepository _constructorRepository;

        public BuildTeamController(
            DriverMockRepository driverRepository,
            ConstructorMockRepository constructorRepository)
        {
            _driverRepository = driverRepository;
            _constructorRepository = constructorRepository;
        }

        public IActionResult Index()
        {
            var model = new BuildFantasyTeamViewModel
            {
                Drivers = _driverRepository.GetAll(),
                Constructors = _constructorRepository.GetAll()
            };

            return View(model);
        }
    }
}