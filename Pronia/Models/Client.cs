namespace Pronia.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public int ProfessionId { get; set; }
        public Profession Profession { get; set; }
    }
}
