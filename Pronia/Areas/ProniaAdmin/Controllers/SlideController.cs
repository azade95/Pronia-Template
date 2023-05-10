using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.DAL;
using Pronia.Models;

namespace Pronia.Areas.ProniaAdmin.Controllers
{
    [Area("ProniaAdmin")]
    public class SlideController : Controller
    {
        private readonly AppDbContext _context;
        public SlideController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            List<Slide> slides = await _context.Slides.ToListAsync();
            return View(slides);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Slide slide)
        {
            if (slide.Photo == null)
            {
                ModelState.AddModelError("Photo", "Photo bos qala bilmez");
                return View();
            }
            if (!slide.Photo.ContentType.Contains("image/"))
            {
                ModelState.AddModelError("Photo", "File tipi dogru deyil. Zehmet olmasa photo secin");
                return View();
            }
            if (slide.Photo.Length > 2048 * 1024)
            {
                ModelState.AddModelError("Photo", "File olcusu 2mbdan boyuk olmamalidir");
                return View();
            }
            if (_context.Slides.Any(s => s.Order == slide.Order))
            {
                ModelState.AddModelError("Order", "Daxil etdiyiniz orderde bir slide movcuddur");
                return View();
            }
            FileStream file = new FileStream(@"C:\Users\ACER\Desktop\pronia\Pronia\Pronia\wwwroot\assets\images\website-images\" + slide.Photo.FileName, FileMode.Create);
            await slide.Photo.CopyToAsync(file);
            slide.Image = slide.Photo.FileName;
            _context.Slides.Add(slide);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null||id<1) return BadRequest();
            Slide existed=await _context.Slides.FirstOrDefaultAsync(s => s.Id == id);
            if (existed == null) return NotFound();
            return View(existed);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id, Slide slide)
        {
            if (id == null || id < 1) return BadRequest();
            Slide existed = await _context.Slides.FirstOrDefaultAsync(s => s.Id == id);
            if (existed == null) return NotFound();


            if (slide.Photo == null)
            {
                ModelState.AddModelError("Photo", "Photo bos qala bilmez");
                return View(existed);
            }
            if (!slide.Photo.ContentType.Contains("image/"))
            {
                ModelState.AddModelError("Photo", "File tipi dogru deyil. Zehmet olmasa photo secin");
                return View(existed);
            }
            if (slide.Photo.Length > 2048 * 1024)
            {
                ModelState.AddModelError("Photo", "File olcusu 2mbdan boyuk olmamalidir");
                return View(existed);
            }
            if (_context.Slides.Any(s => s.Order == slide.Order&&s.Id!=id))
            {
                ModelState.AddModelError("Order", "Daxil etdiyiniz orderde bir slide movcuddur");
                return View(existed);
            }
            existed.Title = slide.Title;
            existed.SubTitle = slide.SubTitle;
            existed.Description = slide.Description;
            existed.Order= slide.Order;
            existed.Photo = slide.Photo;
            existed.Image = slide.Photo.FileName;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");

        }



        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id < 1) return BadRequest();
            Slide existed = await _context.Slides.FirstOrDefaultAsync(s => s.Id == id);
            if (existed == null) return NotFound();
            _context.Slides.Remove(existed);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || id < 1) return BadRequest();
            Slide existed = await _context.Slides.FirstOrDefaultAsync(s => s.Id == id);
            if (existed == null) return NotFound();
            return View(existed);
        }
    }
}
