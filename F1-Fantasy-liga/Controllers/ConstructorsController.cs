using F1_Fantasy_liga.Data;
using F1_Fantasy_liga.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace F1_Fantasy_liga.Controllers
{
    [Route("constructors")]
    public class ConstructorsController : Controller
    {
        private readonly F1DbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ConstructorsController(F1DbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            var constructors = BuildConstructorsQuery(null).ToList();
            return View(constructors);
        }

        [HttpGet("search")]
        public IActionResult Search(string? term)
        {
            var constructors = BuildConstructorsQuery(term).ToList();
            return PartialView("_ConstructorsCards", constructors);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("create")]
        public IActionResult Create()
        {
            return View(new Constructor());
        }

        [HttpGet("autocomplete")]
        public IActionResult Autocomplete(string? term)
        {
            var results = _db.Constructors
                .Where(c => c.IsDeleted == false)
                .Where(c => string.IsNullOrWhiteSpace(term) || c.Name.Contains(term))
                .OrderBy(c => c.Name)
                .Select(c => new { id = c.Id, label = c.Name })
                .Take(20)
                .ToList();

            return Json(results);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Name,Nationality,FoundedDate")] Constructor model, IFormFile? imageFile)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.IsDeleted = false;
            model.DeletedAt = null;
            model.ImagePath = SaveImage(imageFile);

            _db.Constructors.Add(model);
            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("{id:int}")]
        public IActionResult Details(int id)
        {
            var constructor = _db.Constructors
                .Include(c => c.Drivers.Where(d => d.IsDeleted == false))
                .ThenInclude(d => d.RaceResults.Where(rr => rr.IsDeleted == false && rr.Race != null && rr.Race.IsDeleted == false))
                .FirstOrDefault(c => c.Id == id && c.IsDeleted == false);
            if (constructor is null)
            {
                return NotFound();
            }

            return View(constructor);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("edit/{id:int}")]
        public IActionResult Edit(int id)
        {
            var constructor = _db.Constructors.FirstOrDefault(c => c.Id == id && c.IsDeleted == false);
            if (constructor is null)
            {
                return NotFound();
            }

            return View(constructor);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Name,Nationality,FoundedDate,ImagePath")] Constructor model, IFormFile? imageFile)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var constructor = _db.Constructors.FirstOrDefault(c => c.Id == id && c.IsDeleted == false);
            if (constructor is null)
            {
                return NotFound();
            }

            constructor.Name = model.Name;
            constructor.Nationality = model.Nationality;
            constructor.FoundedDate = model.FoundedDate;

            var newImagePath = SaveImage(imageFile);
            if (!string.IsNullOrWhiteSpace(newImagePath))
            {
                DeleteImageFile(constructor.ImagePath);
                constructor.ImagePath = newImagePath;
            }

            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("delete/{id:int}")]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var constructor = _db.Constructors.FirstOrDefault(c => c.Id == id && c.IsDeleted == false);
            if (constructor is null)
            {
                return NotFound();
            }

            constructor.IsDeleted = true;
            constructor.DeletedAt = DateTime.UtcNow;
            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        private IQueryable<Constructor> BuildConstructorsQuery(string? term)
        {
            var query = _db.Constructors
                .Where(c => c.IsDeleted == false)
                .Include(c => c.Drivers.Where(d => d.IsDeleted == false))
                .ThenInclude(d => d.RaceResults.Where(rr => rr.IsDeleted == false && rr.Race != null && rr.Race.IsDeleted == false))
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(term))
            {
                query = query.Where(c => c.Name.Contains(term));
            }

            return query;
        }

        private string SaveImage(IFormFile? imageFile)
        {
            if (imageFile is null || imageFile.Length == 0)
            {
                return string.Empty;
            }

            var imagesFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
            Directory.CreateDirectory(imagesFolder);

            var extension = Path.GetExtension(imageFile.FileName);
            var fileName = $"{Guid.NewGuid():N}{extension}";
            var physicalPath = Path.Combine(imagesFolder, fileName);

            using var stream = new FileStream(physicalPath, FileMode.Create);
            imageFile.CopyTo(stream);

            return "/images/" + fileName;
        }

        private void DeleteImageFile(string? imagePath)
        {
            if (string.IsNullOrWhiteSpace(imagePath))
            {
                return;
            }

            var relativePath = imagePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
            var physicalPath = Path.Combine(_webHostEnvironment.WebRootPath, relativePath);
            if (System.IO.File.Exists(physicalPath))
            {
                System.IO.File.Delete(physicalPath);
            }
        }
    }
}