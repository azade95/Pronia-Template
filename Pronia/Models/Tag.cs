using System.ComponentModel.DataAnnotations;

namespace Pronia.Models
{
    public class Tag
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Ad bosh ola bilmez")]
        [MaxLength(25, ErrorMessage = "Uzunluq 25den chox olmamalidir")]
        public string Name { get; set; }
        public List<ProductTag>? ProductTags { get; set; }

    }
}
