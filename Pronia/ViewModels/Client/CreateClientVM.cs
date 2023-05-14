using System.ComponentModel.DataAnnotations;

namespace Pronia.ViewModels
{
    public class CreateClientVM
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        [Required]
        public int ProfessionId { get; set; }

        public IFormFile Photo { get; set; }
    }
}
