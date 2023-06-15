using System;
using System.Collections.Generic;

namespace wmvc.Models;

public partial class Generator
{
    public int Num { get; set; }

    public string? Name { get; set; }

    public int? Node { get; set; }

    public double? P { get; set; }

    public double? Pmax { get; set; }

    public double? Pmin { get; set; }

    public int? Pgconst { get; set; }

    public int? Type { get; set; }

    public double? Qmax { get; set; }

    public double? Qmin { get; set; }

    public double? Vgain { get; set; }

    public double? Vdrop { get; set; }

    public int? NodeState { get; set; }

    public int? NumXop { get; set; }

    public double? P2 { get; set; }

    public double? Tarif { get; set; }

    public int? Sta { get; set; }

    public double? Pnom { get; set; }

    public int? NumPq { get; set; }

    public string? Adjpq { get; set; }

    public double? Pdem { get; set; }

    public double? Q { get; set; }

    public double? Xd { get; set; }

    public double? CosFi { get; set; }

    public double? Ugnom { get; set; }

    public double? Ki { get; set; }

    public double? Ke { get; set; }

    public int? Ngou { get; set; }

    public double? Bmin { get; set; }

    public double? TgA { get; set; }

    public int? NumBrand { get; set; }

    public string? Sg { get; set; }

    public double? Kct { get; set; }

    public int? NumKi { get; set; }

    public int? NumKe { get; set; }

    public double? Tover { get; set; }

    public int? DispNum { get; set; }

    public double? TiP { get; set; }

    public double? TiQ { get; set; }

    public int? IdGenSql { get; set; }

    public virtual Node? NodeNavigation { get; set; }
}
