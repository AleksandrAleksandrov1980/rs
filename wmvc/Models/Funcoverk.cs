using System;
using System.Collections.Generic;

namespace wmvc.Models;

public partial class Funcoverk
{
    public int SqlId { get; set; }

    public int? Num { get; set; }

    public double? T { get; set; }

    public double? K { get; set; }

    public string? Desc { get; set; }
}
