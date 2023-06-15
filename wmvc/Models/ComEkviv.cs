using System;
using System.Collections.Generic;

namespace wmvc.Models;

public partial class ComEkviv
{
    public int Nra { get; set; }

    public int? Selekv { get; set; }

    public int? Ekvgen { get; set; }

    public int? PotGen { get; set; }

    public double? Kpn { get; set; }

    public double? Kpg { get; set; }

    public double? Zmax { get; set; }

    public int? EkSh { get; set; }

    public int? OtmN { get; set; }

    public int? Smart { get; set; }

    public int? TipGen { get; set; }

    public int? TipSxn { get; set; }

    public int? MetEkv { get; set; }

    public int? TipEkv { get; set; }

    public double? KfcX { get; set; }
}
