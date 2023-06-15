using System;
using System.Collections.Generic;

namespace wmvc.Models;

public partial class KocBaseRg
{
    public int Ny { get; set; }

    public double? Pnzb { get; set; }

    public double? Qnzb { get; set; }

    public double? Pgzb { get; set; }

    public double? Qgzb { get; set; }

    public double? Uzbr { get; set; }

    public double? Urb { get; set; }

    public string? Tuz { get; set; }

    public int? Na { get; set; }

    public double? Pnpti { get; set; }

    public double? Pgpti { get; set; }

    public double? Qnpti { get; set; }

    public double? Qgpti { get; set; }

    public string? Name { get; set; }

    public int? Podsxem { get; set; }

    public double? PBase { get; set; }

    public double? PTek { get; set; }

    public double? POtkl { get; set; }

    public double? POc { get; set; }

    public double? POcOtkl { get; set; }

    public double? PnptiKpod { get; set; }

    public double? QnptiKpod { get; set; }

    public double? PnptiVidel { get; set; }

    public double? QnptiVidel { get; set; }

    public int? SourceP { get; set; }

    public int? SourceQ { get; set; }

    public virtual Area? NaNavigation { get; set; }
}
