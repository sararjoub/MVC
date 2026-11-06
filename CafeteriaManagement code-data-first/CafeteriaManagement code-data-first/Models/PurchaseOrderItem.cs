using System;
using System.Collections.Generic;

namespace CafeteriaManagement_code_data_first.Models;

public partial class PurchaseOrderItem
{
    public int PurchaseOrderItemsId { get; set; }

    public int PurchaseOrderId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual PurchaseOrder PurchaseOrder { get; set; } = null!;
}
