using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.DAL;
using Pronia.Models;

namespace Pronia.Areas.ProniaAdmin.Controllers
{
    [Area("ProniaAdmin")]
    public class TagController : Controller
    {
        AppDbContext _context;
        public TagController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            List<Tag> tags = await _context.Tags.ToListAsync();
            return View(tags);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Tag tag)
        {
            if (!ModelState.IsValid) return View();
            bool result = _context.Tags.Any(t => t.Name.Trim().ToLower() == tag.Name.Trim().ToLower());
            if (result)
            {
                ModelState.AddModelError("Name", "Bu adda tag  movcuddur");
                return View();
            }
            await _context.Tags.AddAsync(tag);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null || id < 1) return BadRequest();
            Tag existed = await _context.Tags.FirstOrDefaultAsync(t => t.Id == id);
            if (existed == null) return NotFound();
            return View(existed);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id, Tag tag)
        {
            if (id == null || id < 1) return BadRequest();
            Tag existed = await _context.Tags.FirstOrDefaultAsync(t => t.Id == id);
            if (existed == null) return NotFound();
            if (!ModelState.IsValid) return View(existed);
            bool result = _context.Tags.Any(t => t.Name.Trim().ToLower() == tag.Name.Trim().ToLower() && t.Id != id);
            if (result)
            {
                ModelState.AddModelError("Name", "Bu adda tag  movcuddur");
                return View(existed);
            }
            existed.Name = tag.Name;
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }



        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id < 1) return BadRequest();
            Tag existed = await _context.Tags.FirstOrDefaultAsync(t => t.Id == id);
            if (existed == null) return NotFound();
            _context.Tags.Remove(existed);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
