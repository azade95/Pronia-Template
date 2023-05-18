using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Pronia.DAL;
using Pronia.Models;
using Pronia.ViewModels;

namespace Pronia.Services
{
    public class LayoutService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _http;

        public LayoutService(AppDbContext context,IHttpContextAccessor http)
        {
            _context = context;
            _http = http;
        }

        public async Task<Dictionary<string,string>> GetSettingsAsync()
        {
            var settings =await _context.Settings.ToDictionaryAsync(s=>s.Key,s=>s.Value);
            return settings;
        }
        public async Task<List<BasketItemVM>> GetBasket()
        {
            List<BasketCookiesItemVM> basket;
            string json =_http.HttpContext.Request.Cookies["basket"];
            if (!String.IsNullOrEmpty(json))
            {
                basket = JsonConvert.DeserializeObject<List<BasketCookiesItemVM>>(json);
            }
            else
            {
                basket = new List<BasketCookiesItemVM>();
            }
            List<BasketItemVM> basketItems = new List<BasketItemVM>();
            List<BasketCookiesItemVM> deleted = new List<BasketCookiesItemVM>();
            foreach (var cookie in basket)
            {
                Product product = await _context.Products.Include(p => p.ProductImages.Where(pi => pi.IsPrimary == true)).FirstOrDefaultAsync(p => p.Id == cookie.Id);
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

            return basketItems;
        }
    }
}
