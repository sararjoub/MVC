namespace UniversityCafeteriaDay6.Models
{
    public class Order
    {
        public int Id { get; set; } 
        public string CustomerName { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; } 
        public string Status { get; set; }= string.Empty;
        public int EmployeeId { get; set; } // يعني الموظف الواحد ممكن يكون مسؤول عن عدة طلبات، لكن كل طلب مرتبط بموظف واحد.
        public Employee Employee { get; set; } = null!; // Navigation Property للوصول لبيانات الموظف

    }
}

