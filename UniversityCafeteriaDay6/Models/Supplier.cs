namespace UniversityCafeteriaDay6.Models
{
    public class Supplier
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string  Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty; // task 7
        public ICollection<Product> Products { get; set; }
    = new List<Product>();
    }
}
