using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            ShopVM shopVM= new ShopVM
            {

                 Product = await _context.Products
                .Include(p=>p.ProductImages)
                .Include(p=>p.Category)
                .Include(p=>p.ProductTags).ThenInclude(pt=>pt.Tag)
                .Include(p=>p.ProductColors).ThenInclude(pc=>pc.Color)
                .Include(p=>p.ProductSizes).ThenInclude(ps=>ps.Size)
                .FirstOrDefaultAsync(p=>p.Id==id),
                Products = await _context.Products.Include(p => p.ProductImages).ToListAsync()

            };
            if (shopVM.Product == null) return NotFound(); 

            return View(shopVM);
        }
    }
}
