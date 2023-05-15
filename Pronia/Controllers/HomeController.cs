using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.DAL;
using Pronia.ViewModels;

namespace Pronia.Controllers
{
    public class HomeController : Controller
    {
        AppDbContext _context;
        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public  async Task<IActionResult> Index()
        {
            HomeVM homeVM = new HomeVM
            {
                Slides=_context.Slides.OrderBy(s => s.Order).Take(2).ToList(),
                Products=await _context.Products.Include(p=>p.ProductImages).OrderByDescending(p=>p.Id).ToListAsync(),
                Clients= await _context.Clients.Include(c=>c.Profession).ToListAsync()
                
            };


            return View(homeVM);
        }
    }
}
