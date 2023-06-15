using System;
using System.Collections.Generic;

namespace wmvc.Models;

public partial class Load
{
    public int Num { get; set; }

    public int? Node { get; set; }

    public double? Q { get; set; }

    public double? P { get; set; }

    public int? Consumer { get; set; }

    public string? Name { get; set; }

    public double? Cos { get; set; }

    public string? Nam2 { get; set; }

    public double? NodeDose { get; set; }

    public int? NodeState { get; set; }

    public int? Sta { get; set; }

    public double? Pmin { get; set; }

    public double? Pmax { get; set; }

    public double? Qmin { get; set; }

    public double? Qmax { get; set; }

    public int? Nsx { get; set; }

    public int? Sel { get; set; }

    public double? Qr { get; set; }

    public double? Pr { get; set; }

    public int? Tpo { get; set; }

    public int? Const { get; set; }

    public int? Na { get; set; }

    public int? Npa { get; set; }

    public virtual Node? NodeNavigation { get; set; }

    public virtual Polin? NsxNavigation { get; set; }
}
