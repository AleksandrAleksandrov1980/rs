using System;
using System.Collections.Generic;

namespace wmvc.Models;

public partial class Reactor
{
    public int Id { get; set; }

    public int? Sta { get; set; }

    public int? Sel { get; set; }

    public string? Name { get; set; }

    public int? Tip { get; set; }

    public int? Id1 { get; set; }

    public int? Id2 { get; set; }

    public int? Id3 { get; set; }

    public int? PrVikl { get; set; }

    public int? PrIzm { get; set; }

    public double? Qr { get; set; }

    public double? Pr { get; set; }

    public double? Qnom { get; set; }

    public double? Pnom { get; set; }

    public double? Unom { get; set; }

    public double? G { get; set; }

    public double? B { get; set; }

    public int? DispNum { get; set; }

    public int? IdReacSql { get; set; }
}
