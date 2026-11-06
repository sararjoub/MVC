using System;
using System.Collections.Generic;

namespace CafeteriaManagement_code_data_first.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    public int Quantity { get; set; }

    public int CategId { get; set; }

    public int SupplierId { get; set; }

    public virtual Category Categ { get; set; } = null!;

    public virtual ICollection<PurchaseOrderItem> PurchaseOrderItems { get; set; } = new List<PurchaseOrderItem>();

    public virtual Supplier Supplier { get; set; } = null!;
}
