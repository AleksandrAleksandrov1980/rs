using System;
using System.Collections.Generic;

namespace wmvc.Models;

public partial class Ancapf
{
    public int Nbd { get; set; }

    public string? Name { get; set; }

    public int? Ei { get; set; }

    public int? Pm { get; set; }

    public int? Tip { get; set; }

    public int? Kne { get; set; }

    public double? Vnr { get; set; }

    public double? Vreg { get; set; }

    public int? NAnc1 { get; set; }

    public double? Sh1 { get; set; }

    public int? NAnc2 { get; set; }

    public double? Sh2 { get; set; }

    public int? NAnc3 { get; set; }

    public double? Sh3 { get; set; }

    public int? NAnc4 { get; set; }

    public double? Sh4 { get; set; }

    public int? NAnc5 { get; set; }

    public double? Sh5 { get; set; }

    public int? NAnc6 { get; set; }

    public double? Sh6 { get; set; }

    public int? NAnc7 { get; set; }

    public double? Sh7 { get; set; }

    public int? NAnc8 { get; set; }

    public double? Sh8 { get; set; }

    public int? NAnc9 { get; set; }

    public double? Sh9 { get; set; }

    public int? NAnc10 { get; set; }

    public double? Sh10 { get; set; }

    public double? KtMin { get; set; }

    public double? KtMax { get; set; }

    public double? KtMid { get; set; }

    public int? Mesto { get; set; }

    public virtual ICollection<Vetv> Vetvs { get; set; } = new List<Vetv>();
}
