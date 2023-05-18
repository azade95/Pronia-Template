using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Pronia.DAL;
using Pronia.Models;
using Pronia.ViewModels;

namespace Pronia.Controllers
{
    public class ShopController : Controller
    {
        AppDbContext _context;
        public ShopController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        
        public async Task<IActionResult> Details(int id)
        {
            Product product = await _context.Products
                .Include(p => p.ProductImages)
                .Include(p => p.Category)
                .Include(p => p.ProductTags).ThenInclude(pt => pt.Tag)
                .Include(p => p.ProductColors).ThenInclude(pc => pc.Color)
                .Include(p => p.ProductSizes).ThenInclude(ps => ps.Size)
                .FirstOrDefaultAsync(p => p.Id == id);
            List<Product> products = await _context.Products.Where(p => p.CategoryId == product.CategoryId && p.Id != product.Id).Include(p => p.ProductImages).ToListAsync();
            ShopVM shopVM= new ShopVM
            {

                 Product =product,
                Products = products

            };
            if (shopVM.Product == null) return NotFound(); 

            return View(shopVM);
        }

        
    }
}
