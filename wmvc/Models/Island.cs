using System;
using System.Collections.Generic;

namespace wmvc.Models;

public partial class Island
{
    public int Nby { get; set; }

    public int? Count { get; set; }

    public double? S { get; set; }

    public double? F { get; set; }

    public double? Pg { get; set; }

    public double? Pgr { get; set; }

    public double? PgMin { get; set; }

    public double? PgMax { get; set; }
}
