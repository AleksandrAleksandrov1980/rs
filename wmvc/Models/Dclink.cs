using System;
using System.Collections.Generic;

namespace wmvc.Models;

public partial class Dclink
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int? ModelType { get; set; }

    public int? Brand { get; set; }

    public int? Sta { get; set; }

    public int? NodeId { get; set; }

    public int? NodeIdQ { get; set; }

    public double? TpidInt { get; set; }

    public double? Kpid { get; set; }

    public double? Qnom { get; set; }

    public double? Unom { get; set; }

    public int? CustomModel { get; set; }

    public int? Control { get; set; }

    public double? Pust { get; set; }

    public double? Iust { get; set; }

    public double? Vust { get; set; }

    public double? Xr { get; set; }

    public double? Xi { get; set; }

    public double? R { get; set; }

    public double? Ar { get; set; }

    public double? Ai { get; set; }

    public double? Kr { get; set; }

    public double? Ki { get; set; }

    public double? UdR { get; set; }

    public double? UdI { get; set; }

    public double? I { get; set; }

    public double? PR { get; set; }

    public double? PI { get; set; }

    public double? QR { get; set; }

    public double? QI { get; set; }

    public double? ArR { get; set; }

    public double? AiR { get; set; }

    public double? Xcr { get; set; }

    public double? Xkr { get; set; }

    public int? TypeVt { get; set; }

    public double? Xci { get; set; }

    public double? Xki { get; set; }

    public double? P2 { get; set; }

    public double? P3 { get; set; }

    public double? Delta2 { get; set; }

    public double? Delta3 { get; set; }

    public int? Nblk { get; set; }

    public double? UvR { get; set; }

    public double? UvI { get; set; }
}
