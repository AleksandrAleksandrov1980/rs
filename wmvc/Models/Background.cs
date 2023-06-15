using System;
using System.Collections.Generic;

namespace wmvc.Models;

public partial class Background
{
    public int SqlId { get; set; }

    public string? Tabl { get; set; }

    public string? Col { get; set; }

    public int? Onoff { get; set; }

    public int? Byvib { get; set; }

    public string? Vibor { get; set; }

    public string? Gradstr { get; set; }
}
