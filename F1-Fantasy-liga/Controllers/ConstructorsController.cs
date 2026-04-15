using F1_Fantasy_liga.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace F1_Fantasy_liga.Controllers
{
    public class ConstructorsController : Controller
    {
        private readonly ConstructorMockRepository _constructorRepository;

        public ConstructorsController(ConstructorMockRepository constructorRepository)
        {
            _constructorRepository = constructorRepository;
        }

        public IActionResult Index()
        {
            var constructors = _constructorRepository.GetAll();
            return View(constructors);
        }

        public IActionResult Details(int id)
        {
            var constructor = _constructorRepository.GetById(id);
            if (constructor is null)
            {
                return NotFound();
            }

            return View(constructor);
        }
    }
}