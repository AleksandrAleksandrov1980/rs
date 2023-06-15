using System;
using System.Collections.Generic;

namespace wmvc.Models;

public partial class Gou
{
    public int Nu { get; set; }

    public string? Name { get; set; }

    public double? Pg { get; set; }

    public double? PgMin { get; set; }

    public double? PgMax { get; set; }

    public double? Pgram { get; set; }

    public string? Vb { get; set; }

    public int? Mode { get; set; }

    public double? Reserve { get; set; }
}
