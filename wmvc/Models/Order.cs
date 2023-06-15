using System;
using System.Collections.Generic;

namespace wmvc.Models;

public partial class Order
{
    public int Customerid { get; set; }

    public int? Quantity { get; set; }

    public virtual Customer Customer { get; set; } = null!;
}
