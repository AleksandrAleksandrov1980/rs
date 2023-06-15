using System;
using System.Collections.Generic;

namespace wmvc.Models;

public partial class CurrentLimit
{
    public int Num { get; set; }

    public int? Sel { get; set; }

    public int? Sta { get; set; }

    public int? Group { get; set; }

    public int? Type { get; set; }

    public int? ElType { get; set; }

    public string? Name { get; set; }

    public double? Duration { get; set; }

    public double? UIp { get; set; }

    public double? UIq { get; set; }

    public int? Ip { get; set; }

    public int? Iq { get; set; }

    public int? Np { get; set; }

    public string? VetvName { get; set; }

    public double? Ib { get; set; }

    public double? Ie { get; set; }

    public int? Msi { get; set; }

    public int? NIt { get; set; }

    public double? IDop { get; set; }

    public double? IDopR { get; set; }

    public double? ZagI { get; set; }

    public virtual Node? IpNavigation { get; set; }

    public virtual Node? IqNavigation { get; set; }
}
