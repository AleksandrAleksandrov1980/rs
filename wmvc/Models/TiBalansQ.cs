using System;
using System.Collections.Generic;

namespace wmvc.Models;

public partial class TiBalansQ
{
    public int SqlId { get; set; }

    public int? TiIp { get; set; }

    public int? TiIq { get; set; }

    public int? TiNp { get; set; }

    public string? Name { get; set; }

    public double? QlshIp { get; set; }

    public double? QlshIq { get; set; }

    public double? QnshDqvet { get; set; }

    public double? QnodeQvet { get; set; }

    public double? Dq { get; set; }

    public int? Sel { get; set; }

    public int? NtiIp { get; set; }

    public int? NtiIq { get; set; }

    public int? NtiItog { get; set; }

    public double? QnQlIp { get; set; }

    public double? QgQlIq { get; set; }

    public double? QlreacIp { get; set; }

    public double? QlreacIq { get; set; }
}
