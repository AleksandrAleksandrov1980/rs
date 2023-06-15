using System;
using System.Collections.Generic;

namespace wmvc.Models;

public partial class AdvgridForm
{
    public int SqlId { get; set; }

    public int? Num { get; set; }

    public string? FormName { get; set; }

    public int? FormString { get; set; }

    public string? SubForm { get; set; }
}
