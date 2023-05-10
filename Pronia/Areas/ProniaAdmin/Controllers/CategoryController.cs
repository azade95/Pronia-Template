using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.DAL;
using Pronia.Models;

namespace Pronia.Areas.ProniaAdmin.Controllers
{
    [Area("ProniaAdmin")]
    public class CategoryController : Controller
    {
        public AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            List<Category> categories = await _context.Categories.Include(c => c.Products).ToListAsync();
            return View(categories);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            if (!ModelState.IsValid) return View();
            bool result = _context.Categories.Any(c => c.Name.Trim().ToLower() == category.Name.Trim().ToLower());
            if (result)
            {
                ModelState.AddModelError("Name", "Bu adda category  movcuddur");
                return View();
            }
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null || id < 1) return BadRequest();
            Category existed = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (existed == null) return NotFound();
            return View(existed);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id,Category category)
        {
            if (id == null || id < 1) return BadRequest();
            Category existed = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (existed == null) return NotFound();
            if (!ModelState.IsValid) return View(existed);
            bool result = _context.Categories.Any(c => c.Name.Trim().ToLower() == category.Name.Trim().ToLower() && c.Id!=id);
            if (result)
            {
                ModelState.AddModelError("Name", "Bu adda category  movcuddur");
                return View(existed);
            }
            existed.Name = category.Name;
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }



        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id < 1) return BadRequest();
            Category existed = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (existed == null) return NotFound();
            _context.Categories.Remove(existed);
            await  _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || id < 1) return BadRequest();
            Category existed = await _context.Categories.Include(c=>c.Products).FirstOrDefaultAsync(c => c.Id == id);
            if (existed == null) return NotFound();
            return View(existed);
        }
    }
}
