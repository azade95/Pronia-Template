namespace Pronia.ViewModels
{
    public class UpdateBlogVM
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string? Image { get; set; }
        public int AuthorId { get; set; }
        public IFormFile? Photo { get; set; }
    }
}
