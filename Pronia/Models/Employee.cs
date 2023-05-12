namespace Pronia.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int PositionId { get; set; }
        public Position Position { get; set; }
    }
}
