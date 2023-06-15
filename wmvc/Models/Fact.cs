using System;
using System.Collections.Generic;

namespace wmvc.Models;

public partial class Fact
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int? Sta { get; set; }

    public int? NodeId { get; set; }

    public int? NodeIdQ { get; set; }

    public double? Snom { get; set; }

    public double? Unom { get; set; }

    public int? Type { get; set; }

    public int? Tref1 { get; set; }

    public double? Ref1 { get; set; }

    public int? Izm { get; set; }

    public double? Min { get; set; }

    public double? Max { get; set; }

    public double? Xl { get; set; }

    public int? Mode { get; set; }

    public int? Tref2 { get; set; }

    public double? Ref2 { get; set; }

    public int? Dcnode { get; set; }

    public double? Xr { get; set; }

    public double? Pr { get; set; }

    public double? Qr { get; set; }

    public double? V { get; set; }

    public double? Delta { get; set; }

    public double? Up { get; set; }

    public double? Uq { get; set; }

    public double? Rr { get; set; }
}
