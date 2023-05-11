using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.DAL;
using Pronia.Models;
using Pronia.Utilities.Exstensions;

namespace Pronia.Areas.ProniaAdmin.Controllers
{
    [Area("ProniaAdmin")]
    public class SlideController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SlideController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
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
            if (!slide.Photo.CheckFileType("image/"))
            {
                ModelState.AddModelError("Photo", "File tipi dogru deyil. Zehmet olmasa photo secin");
                return View();
            }
            if (slide.Photo.CheckFileLength(2048))
            {
                ModelState.AddModelError("Photo", "File olcusu 2mbdan boyuk olmamalidir");
                return View();
            }
            if (_context.Slides.Any(s => s.Order == slide.Order))
            {
                ModelState.AddModelError("Order", "Daxil etdiyiniz orderde bir slide movcuddur");
                return View();
            }
            slide.Image = await slide.Photo.CreateFile(_env.WebRootPath, @"admin/images/slide/");
            await _context.AddAsync(slide);
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


            if (!ModelState.IsValid)
            {
                return View(existed);
            }
            if (_context.Slides.Any(s => s.Order == slide.Order))
            {
                Slide sameOrderSlide = await _context.Slides.FirstOrDefaultAsync(s => s.Order == slide.Order && s.Id != id);
                int sameOrder = sameOrderSlide.Order;
                sameOrderSlide.Order = existed.Order;
                existed.Order = slide.Order;

            }

            if (slide.Photo != null)
            {
                if (!slide.Photo.CheckFileType("image/"))
                {
                    ModelState.AddModelError("Photo", "File tipi duzgun deyil");
                    return View(existed);
                }
                if (!slide.Photo.CheckFileLength(2048))
                {
                    ModelState.AddModelError("Photo", "File olcusu 2mbdan cox olmamalidir");
                    return View(existed);
                }
                existed.Image.DeleteFile(_env.WebRootPath, @"admin/images/slide/");
                existed.Image = await slide.Photo.CreateFile(_env.WebRootPath, @"admin/images/slide/");
            }
            existed.Title = slide.Title;
            existed.SubTitle = slide.SubTitle;
            existed.Description = slide.Description;
            
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
