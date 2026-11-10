namespace Task_8.Models
{
    public class RequestItem
    {
        public int Id { get; set; }

        public int SupplyRequestId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public SupplyRequest SupplyRequest { get; set; } = null!;

        public Product Product { get; set; } = null!;
    }
}

// كل SupplyRequest ممكن يحتوي على عدة RequestItems
// كل Product ممكن يظهر في عدة RequestItems تابعة لطلبات مختلفة
// كل RequestItem مرتبط بطلب واحد ومنتج واحد