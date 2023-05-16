using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.DAL;
using Pronia.Models;
using Pronia.Utilities.Exstensions;
using Pronia.ViewModels;

namespace Pronia.Areas.ProniaAdmin.Controllers
{
    [Area("ProniaAdmin")]
    public class BlogController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public BlogController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<Blog> blogs = await _context.Blogs.ToListAsync();
            return View(blogs);
        }

        public IActionResult Create()
        {
            ViewBag.Authors= _context.Authors.ToList();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateBlogVM blogVM)
        {
            if(!ModelState.IsValid)
            {
                ViewBag.Authors= _context.Authors.ToList();
                return View(blogVM);
            }
            if (!blogVM.Photo.CheckFileType("image/"))
            {
                ModelState.AddModelError("Photo", "File tipi dogru deyil");
                ViewBag.Authors= _context.Authors.ToList();
                return View(blogVM);
            }
            if (!blogVM.Photo.CheckFileLength(2048))
            {
                ModelState.AddModelError("Photo", "File olchusu 2mbdan chox olmamalidir");
                ViewBag.Authors = _context.Authors.ToList();
                return View(blogVM);
            }
            Blog blog=new Blog
            {
                Title = blogVM.Title,
                Description = blogVM.Description,
                Image=await blogVM.Photo.CreateFile(_env.WebRootPath, @"assets/images/website-images/banner"),
                CreatedAt = DateTime.Now
            };
            _context.Blogs.Add(blog);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null || id < 1) return BadRequest();
            Blog existed = await _context.Blogs.FirstOrDefaultAsync(b => b.Id == id);
            if(existed == null) return NotFound();
            UpdateBlogVM blogVM = new UpdateBlogVM
            {
                Title= existed.Title,
                Description= existed.Description,
                Image = existed.Image
                
            };
            return View(blogVM);

        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id, UpdateBlogVM blogVM)
        {
            if (id == null || id < 1) return BadRequest();
            Blog existed = await _context.Blogs.FirstOrDefaultAsync(b => b.Id == id);
            if (existed == null) return NotFound();
            if (!ModelState.IsValid)
            {
                return View(blogVM);
            }
           if(blogVM.Photo != null)
            {
                if (!blogVM.Photo.CheckFileType("image/"))
                {
                    ModelState.AddModelError("Photo", "File tipi dogru deyil");
                    return View(blogVM);
                }
                if (!blogVM.Photo.CheckFileLength(2048))
                {
                    ModelState.AddModelError("Photo", "File olchusu 2mbdan chox olmamalidir");
                    return View(blogVM);
                }
                existed.Image.DeleteFile(_env.WebRootPath, @"assets/images/website-images/blog");
                existed.Image =await blogVM.Photo.CreateFile(_env.WebRootPath, @"assets/images/website-images/blog");
            }

            existed.Title = blogVM.Title;
            existed.Description = blogVM.Description;
            existed.UpdatedAt = DateTime.Now;
            _context.Blogs.Update(existed);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");

        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id < 1) return BadRequest();
            Blog existed = await _context.Blogs.FirstOrDefaultAsync(b => b.Id == id);
            if (existed == null) return NotFound();
            _context.Blogs.Remove(existed);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
