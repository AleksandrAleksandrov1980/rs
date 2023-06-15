using System;
using System.Collections.Generic;

namespace wmvc.Models;

public partial class Diff
{
    public int DiffIt { get; set; }

    public string? Tabl { get; set; }

    public string? Col { get; set; }

    public string? Addit { get; set; }

    public string? Vib { get; set; }
}
