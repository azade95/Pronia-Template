using Microsoft.AspNetCore.Mvc;
using Pronia.DAL;
using Pronia.Models;

namespace Pronia.ViewComponents
{
    public class ProductViewComponent:ViewComponent
    {
        private readonly AppDbContext _context;

        public ProductViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int key)
        {
            List<Product> products = new List<Product>();
            switch (key)
            {
                case 1: products = _context.Products.OrderBy(p => p.Name).ToList(); break;
                case 2: products = _context.Products.OrderBy(p => p.Price).ToList(); break;
                case 3: products = _context.Products.OrderByDescending(p => p.Id).ToList(); break;
                default:
                    products = _context.Products.ToList();
                    break;
            }

            //List<Product> products = _context.Products.OrderBy(p => p.Price).ToList();
            return View(products);
        }
    }
}
