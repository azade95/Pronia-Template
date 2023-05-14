using System.ComponentModel.DataAnnotations;

namespace Pronia.ViewModels.Banner
{
    public class CreateBannerVM
    {
        [Required(ErrorMessage ="Title hissesi bosh qala bilmez")]
        public string Title { get; set; }
        public string SubTitle { get; set; }
        [Required(ErrorMessage ="shekil hissesi bosh qala bilmez")]
        public IFormFile Photo { get; set; }
    }
}
