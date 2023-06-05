using System.ComponentModel.DataAnnotations;

namespace Pronia.ViewModels
{
    public class UpdateProductVM
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public string SKU { get; set; }
        public int CategoryId { get; set; }
        public List<int> TagIds { get; set; }


        //public IFormFile PrimaryPhoto { get; set; }
        //public IFormFile SecondaryPhoto { get; set; }
        //public List<IFormFile> Photos { get; set; }
    }
}
