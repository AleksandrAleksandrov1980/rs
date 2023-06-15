using System;
using System.Collections.Generic;

namespace wmvc.Models;

public partial class Background4
{
    public int SqlId { get; set; }

    public string? Tabl { get; set; }

    public string? Col { get; set; }

    public string? Expr { get; set; }

    public string? Gradstr { get; set; }

    public int? Isgrad { get; set; }

    public int? Onoff { get; set; }
}
