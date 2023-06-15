using System;
using System.Collections.Generic;

namespace wmvc.Models;

public partial class Darea
{
    public int No { get; set; }

    public string? Name { get; set; }

    public double? Pg { get; set; }

    public double? Pp { get; set; }

    public double? Pvn { get; set; }

    public double? Qg { get; set; }

    public double? Qp { get; set; }

    public double? Qvn { get; set; }

    public string? Tmpn { get; set; }

    public string? Tmp2 { get; set; }

    public double? Dp { get; set; }

    public double? Dq { get; set; }

    public string? Tmpun { get; set; }

    public double? DpNagr { get; set; }

    public double? DpLine { get; set; }

    public double? DpTran { get; set; }

    public double? DpXx { get; set; }

    public double? ShLine { get; set; }

    public double? ShTran { get; set; }

    public double? ShShunt { get; set; }

    public double? DqNagr { get; set; }

    public double? DqLine { get; set; }

    public double? DqTran { get; set; }

    public double? DqXx { get; set; }

    public double? ShqLine { get; set; }

    public double? ShqTran { get; set; }

    public double? ShqShunt { get; set; }

    public double? Tc { get; set; }

    public int? MeteoId { get; set; }

    public int? Sel { get; set; }
}
