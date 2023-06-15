using System;
using System.Collections.Generic;

namespace wmvc.Models;

public partial class Sendcommandmainform
{
    public int SqlId { get; set; }

    public int? Menu { get; set; }

    public string? Submenu { get; set; }

    public int? CommId { get; set; }

    public string? P1 { get; set; }

    public string? P2 { get; set; }

    public int? Pp { get; set; }

    public string? Formname { get; set; }

    public int? Tgtmask { get; set; }
}
