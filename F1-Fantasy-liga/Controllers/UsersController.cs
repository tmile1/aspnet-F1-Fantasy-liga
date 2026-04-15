using F1_Fantasy_liga.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace F1_Fantasy_liga.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserMockRepository _userRepository;

        public UsersController(UserMockRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IActionResult Index()
        {
            var users = _userRepository.GetAll();
            return View(users);
        }

        public IActionResult Details(int id)
        {
            var user = _userRepository.GetById(id);
            if (user is null)
            {
                return NotFound();
            }

            return View(user);
        }
    }
}