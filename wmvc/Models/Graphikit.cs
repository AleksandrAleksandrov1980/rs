using System;
using System.Collections.Generic;

namespace wmvc.Models;

public partial class Graphikit
{
    public int SqlId { get; set; }

    public int? Num { get; set; }

    public double? Tc { get; set; }

    public double? Idop { get; set; }

    public string? Name { get; set; }
}
