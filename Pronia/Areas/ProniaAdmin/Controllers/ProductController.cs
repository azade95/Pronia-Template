using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.DAL;
using Pronia.Models;
using Pronia.Utilities.Exstensions;
using Pronia.ViewModels;


namespace Pronia.Areas.ProniaAdmin.Controllers
{
    [Area("ProniaAdmin")]
    [AutoValidateAntiforgeryToken]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductController(AppDbContext context,IWebHostEnvironment env)
        {
            _context = context;
           _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<Product> products = await _context.Products
                .Include(p=>p.ProductImages
                .Where(pi=>pi.IsPrimary==true))
                .Include(p=>p.Category)
                .Include(p=>p.ProductTags)
                .ThenInclude(pt=>pt.Tag)
                .ToListAsync();
            return View(products);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Categories=await _context.Categories.ToListAsync();
            ViewBag.Tags=await _context.Tags.ToListAsync();
            return View();

        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductVM productVM)
        {
            if(!ModelState.IsValid)
            {
                ViewBag.Categories = await _context.Categories.ToListAsync();
                ViewBag.Tags = await _context.Tags.ToListAsync();
                return View();
            }
            bool result = await _context.Categories.AnyAsync(c => c.Id == productVM.CategoryId);
            if (!result)
            {
                ViewBag.Categories = await _context.Categories.ToListAsync();
                ViewBag.Tags = await _context.Tags.ToListAsync();
                ModelState.AddModelError("CategoryId", "Bu id-li category movcud deyil");
                return View();
            }

            Product product = new Product
            {
                Name = productVM.Name,
                Description = productVM.Description,
                Price = productVM.Price,
                SKU = productVM.SKU,
                CategoryId = productVM.CategoryId,
                ProductTags = new List<ProductTag>(),
                ProductImages = new List<ProductImage>()
            };

            foreach(int tagId in productVM.TagIds)
            {
                bool tagResult= await _context.Tags.AnyAsync(t => t.Id == tagId);
                if (!tagResult)
                {
                    ViewBag.Categories = await _context.Categories.ToListAsync();
                    ViewBag.Tags = await _context.Tags.ToListAsync();
                    ModelState.AddModelError("TagIds", $"{tagId} id-li tag movcud deyil");
                    return View();
                }

                ProductTag productTag = new ProductTag
                {
                    TagId = tagId,
                    Product = product,
                };

                product.ProductTags.Add(productTag);
            }

            if (!productVM.PrimaryPhoto.CheckFileType("image/"))
            {
                ViewBag.Categories = await _context.Categories.ToListAsync();
                ViewBag.Tags = await _context.Tags.ToListAsync();
                ModelState.AddModelError("PrimaryPhoto", "File tipi dogru deyil");
                return View();
            }
            if (!productVM.PrimaryPhoto.CheckFileLength(200))
            {
                ViewBag.Categories = await _context.Categories.ToListAsync();
                ViewBag.Tags = await _context.Tags.ToListAsync();
                ModelState.AddModelError("PrimaryPhoto", "File olchusu 200kbdan chox olmamalidir");
                return View();
            }

            if (!productVM.SecondaryPhoto.CheckFileType("image/"))
            {
                ViewBag.Categories = await _context.Categories.ToListAsync();
                ViewBag.Tags = await _context.Tags.ToListAsync();
                ModelState.AddModelError("SecondaryPhoto", "File tipi dogru deyil");
                return View();
            }
            if (!productVM.SecondaryPhoto.CheckFileLength(200))
            {
                ViewBag.Categories = await _context.Categories.ToListAsync();
                ViewBag.Tags = await _context.Tags.ToListAsync();
                ModelState.AddModelError("SecondaryPhoto", "File olchusu 200kbdan chox olmamalidir");
                return View();
            }

            ProductImage primaryPhoto = new ProductImage
            {
                ImageUrl= await productVM.PrimaryPhoto.CreateFile(_env.WebRootPath,"assets/images/website-images"),
                IsPrimary=true,
                Product=product
            };
            ProductImage secondaryPhoto = new ProductImage
            {
                ImageUrl = await productVM.SecondaryPhoto.CreateFile(_env.WebRootPath, "assets/images/website-images"),
                IsPrimary = false,
                Product = product
            };
            product.ProductImages.Add(primaryPhoto);
            product.ProductImages.Add(secondaryPhoto);

            TempData["PhotoError"] = "";
            foreach (IFormFile photo in productVM.Photos)
            {
                if (!photo.CheckFileType("image/"))
                {
                    TempData["PhotoError"] += $"{photo.FileName} file tipi uygun deyil\t";
                    continue;
                }
                if (!photo.CheckFileLength(200))
                {
                    TempData["PhotoError"] += $"{photo.FileName} file olchusu uygun deyil\t";
                    continue;
                }

                ProductImage addPhoto = new ProductImage
                {
                    ImageUrl = await photo.CreateFile(_env.WebRootPath, "assets/images/website-images"),
                    IsPrimary = null,
                    Product = product
                };

                product.ProductImages.Add(addPhoto);
            }

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null || id < 1) return BadRequest();
            Product product = await _context.Products.Where(p => p.Id == id).Include(p=>p.ProductTags).FirstOrDefaultAsync();
            if (product == null) return NotFound();
            UpdateProductVM productVM = new UpdateProductVM
            {
                Name = product.Name,
                Description = product.Description,
                SKU = product.SKU,
                Price = product.Price,
                CategoryId = product.CategoryId,
                TagIds= product.ProductTags.Select(p => p.TagId).ToList()
            };

            ViewBag.Categories = await _context.Categories.ToListAsync();
            ViewBag.Tags = await _context.Tags.ToListAsync();
            return View(productVM);

        }
    }
}
