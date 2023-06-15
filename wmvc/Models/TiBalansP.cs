using System;
using System.Collections.Generic;

namespace wmvc.Models;

public partial class TiBalansP
{
    public int SqlId { get; set; }

    public int? TiIp { get; set; }

    public int? TiIq { get; set; }

    public int? TiNp { get; set; }

    public string? Name { get; set; }

    public double? PlshIp { get; set; }

    public double? PlshIq { get; set; }

    public double? PnshDpvet { get; set; }

    public double? PnodePvet { get; set; }

    public double? Dp { get; set; }

    public int? Sel { get; set; }

    public int? NtiIp { get; set; }

    public int? NtiIq { get; set; }

    public int? NtiItog { get; set; }

    public double? PnPlIp { get; set; }

    public double? PgPlIq { get; set; }
}
