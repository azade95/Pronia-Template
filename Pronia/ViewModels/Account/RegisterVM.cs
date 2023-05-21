using System.ComponentModel.DataAnnotations;

namespace Pronia.ViewModels
{
    public class RegisterVM
    {
        [Required]
        [MinLength(3,ErrorMessage = "Ad 3den qisa olmamalidir")]
        [MaxLength(25,ErrorMessage = "Ad 25den uzun olmamalidir")]
        public string Name { get; set; }
        [Required]
        [MinLength(3, ErrorMessage = "Soyad 3den qisa olmamalidir")]
        [MaxLength(25, ErrorMessage = "Soyad 25den uzun olmamalidir")]
        public string Surname { get; set; }
        [Required]
        [MinLength(3,ErrorMessage ="Username 3den qisa olmamalidir")]
        [MaxLength(15,ErrorMessage ="Username 15den uzun olmamalidir")]
        public string Username { get; set; }
        [Required]
        [MinLength(3)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }   
        public string  Gender { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [MinLength(8,ErrorMessage ="Password en az 8 simvoldan ibaret olmalidir")]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password),Compare(nameof(Password),ErrorMessage ="Password ve Confirm password eyni deyil")]
        public string ConfirmPassword { get; set; }
    }
}
