using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.DAL;
using Pronia.Models;
using Pronia.Utilities.Exstensions;
using Pronia.ViewModels.Banner;

namespace Pronia.Areas.ProniaAdmin.Controllers
{
    [Area("ProniaAdmin")]
    public class BannerController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public BannerController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<Banner> banners=await _context.Banners.ToListAsync();
            return View(banners);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateBannerVM bannerVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (!bannerVM.Photo.CheckFileType("image/"))
            {
                ModelState.AddModelError("Photo", "File tipi dogru deyil!");
                return View();
            }
            if (!bannerVM.Photo.CheckFileLength(2048))
            {
                ModelState.AddModelError("Photo", "File olchusu 2mbdan chox ola bilmez!");
                return View();
            }
            Banner banner = new Banner
            {
                Title = bannerVM.Title,
                SubTitle = bannerVM.SubTitle,
                Image = await bannerVM.Photo.CreateFile(_env.WebRootPath, @"assets/images/website-images/banner")
            };
            await _context.Banners.AddAsync(banner);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null || id < 1) return BadRequest();
            Banner existed=await _context.Banners.FirstOrDefaultAsync(b=> b.Id == id);
            if (existed == null) return NotFound();
            if (!ModelState.IsValid)
            {
                return View();
            }
            UpdateBannerVM bannerVM=new UpdateBannerVM
            {
                Title=existed.Title,
                SubTitle=existed.SubTitle,
                Image = existed.Image
            };
            return View(bannerVM);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int? id,UpdateBannerVM bannerVM)
        {
            if (id == null || id < 1) return BadRequest();
            Banner existed = await _context.Banners.FirstOrDefaultAsync(b => b.Id == id);
            if (existed == null) return NotFound();
            if (!ModelState.IsValid)
            {
                return View(bannerVM);
            }
            if(bannerVM.Photo!=null)
            {
                if (!bannerVM.Photo.CheckFileType("image/"))
                {
                    ModelState.AddModelError("Photo", "File tipi dogru deyil!");
                    return View(bannerVM);
                }
                if (!bannerVM.Photo.CheckFileLength(2048))
                {
                    ModelState.AddModelError("Photo", "File olchusu 2mbdan chox ola bilmez!");
                    return View(bannerVM);
                }
                existed.Image.DeleteFile(_env.WebRootPath, @"assets/images/website-images/banner");
                existed.Image = await bannerVM.Photo.CreateFile(_env.WebRootPath, @"assets/images/website-images/banner");

            }

            existed.Title = bannerVM.Title;
            existed.SubTitle = bannerVM.SubTitle;

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id < 1) return BadRequest();
            Banner existed = await _context.Banners.FirstOrDefaultAsync(s => s.Id == id);
            if (existed == null) return NotFound();
            _context.Banners.Remove(existed);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

    }
}
