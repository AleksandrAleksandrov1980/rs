using System;
using System.Collections.Generic;

namespace wmvc.Models;

public partial class Consumer
{
    public int Num { get; set; }

    public string? Name { get; set; }

    public int? Type { get; set; }

    public double? Q { get; set; }

    public double? P { get; set; }

    public double? LoadLoss { get; set; }
}
