using System.ComponentModel.DataAnnotations;

namespace Pronia.Models
{
    public class Blog
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set;}
        public int AuthorId { get; set; }
        public Author Author { get; set; }
    }
}
