using System.ComponentModel.DataAnnotations;

namespace Pronia.ViewModels.Banner
{
    public class UpdateBannerVM
    {
        [Required(ErrorMessage = "Title hissesi bosh qala bilmez")]
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Image { get; set; }
       
        public IFormFile? Photo { get; set; }
    }
}
