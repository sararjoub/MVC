using Microsoft.AspNetCore.Identity;

namespace Task_8.Models
{
    public class SupplyRequest
    {
        public int Id { get; set; }

        public DateTime RequestDate { get; set; }

        public decimal TotalPrice { get; set; }

        public string Status { get; set; } = "Pending";

        public string UserId { get; set; } = string.Empty;

        public ApplicationUser User { get; set; } = null!;

        public ICollection<RequestItem> RequestItems { get; set; }
            = new List<RequestItem>();
    }
}
