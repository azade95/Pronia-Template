using Microsoft.EntityFrameworkCore;
using Pronia.Models;

namespace Pronia.DAL
{
    public class AppDbContext:DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {
            
        }
        public DbSet<Slide> Slides { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductTag> ProductTags { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<ProductColor> ProductColors { get; set; }
        public DbSet<ProductSize> ProductSizes { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Profession> Professions { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Banner> Banners { get; set; }
        public DbSet<Blog> Blogs { get; set; }
    }
}
