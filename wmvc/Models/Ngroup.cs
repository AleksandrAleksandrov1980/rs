using System;
using System.Collections.Generic;

namespace wmvc.Models;

public partial class Ngroup
{
    public int Nga { get; set; }

    public string? Name { get; set; }

    public double? Pg { get; set; }

    public double? Pn { get; set; }

    public double? Qg { get; set; }

    public double? Qn { get; set; }

    public double? Dp { get; set; }

    public double? Dq { get; set; }

    public double? Pop { get; set; }

    public double? Poq { get; set; }

    public double? Vnp { get; set; }

    public double? Vnq { get; set; }

    public string? X3 { get; set; }

    public string? NaNgroup { get; set; }

    public string? Navet { get; set; }

    public int? Sel { get; set; }
}
