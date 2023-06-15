using System;
using System.Collections.Generic;

namespace wmvc.Models;

public partial class ComOptim
{
    public int Nopt { get; set; }

    public int? IrmRegul { get; set; }

    public int? Regul { get; set; }

    public int? Anc { get; set; }

    public double? DpotMax { get; set; }

    public double? DshtrMax { get; set; }

    public int? IterMm { get; set; }

    public int? IterMax { get; set; }

    public int? IterBasis { get; set; }

    public double? KoefShtr { get; set; }

    public double? KoefKt { get; set; }

    public double? Ud { get; set; }

    public double? Divider { get; set; }

    public int? MinPot { get; set; }
}
