using System;
using System.Collections.Generic;

namespace CafeteriaManagement_code_data_first.Models;

public partial class Category
{
    public int CategId { get; set; }

    public string? CategName { get; set; }

    public string? CateDescription { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
