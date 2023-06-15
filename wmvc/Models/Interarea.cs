using System;
using System.Collections.Generic;

namespace wmvc.Models;

public partial class Interarea
{
    public int BNa { get; set; }

    public int ENa { get; set; }

    public double? Pb { get; set; }

    public double? Pe { get; set; }

    public string? Najact { get; set; }

    public int? Tmpna { get; set; }

    public string? NaName { get; set; }

    public int? NaNa { get; set; }

    public double? NaPs { get; set; }

    public int? Np { get; set; }

    public int? Sta { get; set; }

    public int? Sel { get; set; }

    public int? Tip { get; set; }

    public int? Zbg { get; set; }

    public int? Zen { get; set; }

    public double? Sb { get; set; }

    public double? Se { get; set; }

    public double? Zero { get; set; }

    public int? BNa2 { get; set; }

    public int? ENa2 { get; set; }

    public double? Pb2 { get; set; }

    public double? Pe2 { get; set; }

    public string? NaName2 { get; set; }

    public int? NaNa2 { get; set; }

    public double? NaPs2 { get; set; }

    public int? BNa3 { get; set; }

    public int? ENa3 { get; set; }

    public double? Pb3 { get; set; }

    public double? Pe3 { get; set; }

    public string? NaName3 { get; set; }

    public int? NaNa3 { get; set; }

    public double? NaPs3 { get; set; }

    public virtual Area BNaNavigation { get; set; } = null!;

    public virtual Area ENaNavigation { get; set; } = null!;
}
