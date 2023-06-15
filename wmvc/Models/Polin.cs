using System;
using System.Collections.Generic;

namespace wmvc.Models;

public partial class Polin
{
    public int Nsx { get; set; }

    public double? P1 { get; set; }

    public double? P2 { get; set; }

    public double? P3 { get; set; }

    public double? P4 { get; set; }

    public double? Q0 { get; set; }

    public double? Q1 { get; set; }

    public double? Q2 { get; set; }

    public double? Q3 { get; set; }

    public double? Q4 { get; set; }

    public double? P0 { get; set; }

    public double? Umin { get; set; }

    public double? Frec { get; set; }

    public double? Frecq { get; set; }

    public int? Sta { get; set; }

    public virtual ICollection<Load> Loads { get; set; } = new List<Load>();
}
