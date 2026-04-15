using F1_Fantasy_liga.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace F1_Fantasy_liga.Controllers
{
    public class CircuitsController : Controller
    {
        private readonly CircuitMockRepository _circuitRepository;

        public CircuitsController(CircuitMockRepository circuitRepository)
        {
            _circuitRepository = circuitRepository;
        }

        public IActionResult Index()
        {
            var circuits = _circuitRepository.GetAll();
            return View(circuits);
        }

        public IActionResult Details(int id)
        {
            var circuit = _circuitRepository.GetById(id);
            if (circuit is null)
            {
                return NotFound();
            }

            return View(circuit);
        }
    }
}