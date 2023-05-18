using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Pronia.DAL;
using Pronia.Models;
using Pronia.ViewModels;

namespace Pronia.Controllers
{
    public class CartController : Controller
    {
        private readonly AppDbContext _context;

        public CartController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            List<BasketCookiesItemVM> basket;
            string json = Request.Cookies["basket"];
            if (!String.IsNullOrEmpty(json))
            {
               basket = JsonConvert.DeserializeObject<List<BasketCookiesItemVM>>(json);
            }
            else
            {
                basket=new List<BasketCookiesItemVM>();
            }
            List<BasketItemVM> basketItems=new List<BasketItemVM>();
            List<BasketCookiesItemVM> deleted = new List<BasketCookiesItemVM>();
            foreach (var cookie in basket)
            {
              Product product = await _context.Products.Include(p => p.ProductImages.Where(pi => pi.IsPrimary == true)).FirstOrDefaultAsync(p=>p.Id==cookie.Id);
               if (product == null)
                {
                    deleted.Add(cookie);
                }
                BasketItemVM itemVM = new BasketItemVM
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Image = product.ProductImages.FirstOrDefault().ImageUrl,
                    Count = cookie.Count
                };
                basketItems.Add(itemVM);
                for (int i = 0; i < deleted.Count; i++)
                {
                    basket.Remove(deleted[i]);
                }
            }
            return View(basketItems);
        }

        public async Task<IActionResult> AddCart(int? id)
        {
            if (id == 0) return BadRequest();
            Product product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null) return NotFound();
            List<BasketCookiesItemVM> basket;
            if (Request.Cookies["Basket"] == null)
            {
                basket = new List<BasketCookiesItemVM>();

                basket.Add(new BasketCookiesItemVM
                {
                    Id = product.Id,
                    Count = 1
                });
            }
            else
            {
                basket = JsonConvert.DeserializeObject<List<BasketCookiesItemVM>>(Request.Cookies["Basket"]);
                BasketCookiesItemVM existed = basket.FirstOrDefault(b => b.Id == id);
                if (existed != null)
                {
                    existed.Count++;
                }
                else
                {
                    basket.Add(new BasketCookiesItemVM
                    {
                        Id = product.Id,
                        Count = 1
                    });
                }

            }


            string json = JsonConvert.SerializeObject(basket);
            Response.Cookies.Append("Basket", json);
            return RedirectToAction(nameof(Index), "Home");

        }
    }
}
