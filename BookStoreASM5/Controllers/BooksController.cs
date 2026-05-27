using BookStoreASM5.Data;
using BookStoreASM5.Models;
using BookStoreASM5.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BookDB_MVC.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BooksController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index(string? search, int? categoryId)
        {
            var query = _context.Books.Include(b => b.Category).AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(b => b.Title.Contains(search) || b.Author.Contains(search));
            }

            if (categoryId.HasValue)
            {
                query = query.Where(b => b.CategoryId == categoryId.Value);
            }

            ViewBag.Search = search;
            ViewBag.CategoryId = categoryId;
            ViewBag.Categories = await _context.Categories.ToListAsync();

            var books = await query.OrderByDescending(b => b.Id).ToListAsync();
            return View(books);
        }

        public async Task<IActionResult> Details(int id)
        {
            var book = await _context.Books
                .Include(b => b.Category)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null) return NotFound();

            return View(book);
        }

        public async Task<IActionResult> Create()
        {
            var vm = new BookFormViewModel
            {
                Categories = await GetCategoriesSelectList()
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookFormViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Categories = await GetCategoriesSelectList();
                return View(vm);
            }

            string? imagePath = null;

            if (vm.ImageFile != null && vm.ImageFile.Length > 0)
            {
                imagePath = await SaveImage(vm.ImageFile);
            }

            var book = new Book
            {
                Title = vm.Title,
                Author = vm.Author,
                Price = vm.Price,
                Description = vm.Description,
                Image = imagePath,
                CategoryId = vm.CategoryId
            };

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null) return NotFound();

            var vm = new BookFormViewModel
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Price = book.Price,
                Description = book.Description,
                CurrentImage = book.Image,
                CategoryId = book.CategoryId,
                Categories = await GetCategoriesSelectList()
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BookFormViewModel vm)
        {
            if (id != vm.Id) return BadRequest();

            if (!ModelState.IsValid)
            {
                vm.Categories = await GetCategoriesSelectList();
                return View(vm);
            }

            var book = await _context.Books.FindAsync(id);
            if (book == null) return NotFound();

            book.Title = vm.Title;
            book.Author = vm.Author;
            book.Price = vm.Price;
            book.Description = vm.Description;
            book.CategoryId = vm.CategoryId;

            if (vm.ImageFile != null && vm.ImageFile.Length > 0)
            {
                if (!string.IsNullOrWhiteSpace(book.Image))
                {
                    DeleteImage(book.Image);
                }

                book.Image = await SaveImage(vm.ImageFile);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var book = await _context.Books
                .Include(b => b.Category)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null) return NotFound();

            return View(book);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null) return NotFound();

            if (!string.IsNullOrWhiteSpace(book.Image))
            {
                DeleteImage(book.Image);
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private async Task<List<SelectListItem>> GetCategoriesSelectList()
        {
            return await _context.Categories
                .Select(c => new SelectListItem
                {
                    Value = c.CategoryId.ToString(),
                    Text = c.CategoryName
                })
                .ToListAsync();
        }

        private async Task<string> SaveImage(IFormFile file)
        {
            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using var fileStream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(fileStream);

            return "/images/" + uniqueFileName;
        }

        private void DeleteImage(string imagePath)
        {
            var fileName = Path.GetFileName(imagePath);
            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", fileName);

            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        }
    }
}