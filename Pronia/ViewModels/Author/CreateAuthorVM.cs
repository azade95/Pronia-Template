using System.ComponentModel.DataAnnotations;

namespace Pronia.ViewModels
{
    public class CreateAuthorVM
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        [Required]
        public string Username { get; set; }
    }
}
