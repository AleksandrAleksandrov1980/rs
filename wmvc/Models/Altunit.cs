using System;
using System.Collections.Generic;

namespace wmvc.Models;

public partial class Altunit
{
    public int Num { get; set; }

    public int? Activ { get; set; }

    public string? Unit { get; set; }

    public string? Alt { get; set; }

    public string? Formula { get; set; }

    public string? Tabl { get; set; }

    public int? Prec { get; set; }
}
