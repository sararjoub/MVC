using System.Xml.Linq;

namespace GraduationSupplyManagement.Models
{
   

    public class SupplyOffer
    {
        public int Id { get; set; }
        public string Category { get; set; } = string.Empty;

        public string SupplierName { get; set; } = string.Empty;

        public string OfferName { get; set; } = string.Empty;
        
        public  int Quantity { get; set; }
        public decimal UnitPrice { get; set; }






    }
}
