using F1_Fantasy_liga.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace F1_Fantasy_liga.Controllers
{
    public class DriversController : Controller
    {
        private readonly DriverMockRepository _driverRepository;

        public DriversController(DriverMockRepository driverRepository)
        {
            _driverRepository = driverRepository;
        }

        public IActionResult Index()
        {
            var drivers = _driverRepository.GetAll();
            return View(drivers);
        }

        public IActionResult Details(int id)
        {
            var driver = _driverRepository.GetById(id);
            if (driver is null)
            {
                return NotFound();
            }

            return View(driver);
        }
    }
}