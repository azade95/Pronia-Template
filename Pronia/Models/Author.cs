using System.ComponentModel.DataAnnotations;

namespace Pronia.Models
{
    public class Author
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        [Required]
        public string Username { get; set; }
        public List<Blog> Blogs { get; set; }
    }
}
