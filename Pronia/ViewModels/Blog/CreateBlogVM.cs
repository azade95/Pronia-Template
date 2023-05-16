using System.ComponentModel.DataAnnotations;

namespace Pronia.ViewModels
{
    public class CreateBlogVM
    {
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public int AuthorId { get; set; }   
        public IFormFile Photo { get; set; }
    }
}
